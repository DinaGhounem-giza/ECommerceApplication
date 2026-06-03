using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AbpSolution1.Categories;
using AbpSolution1.DTOs.Orders;
using AbpSolution1.Orders;
using AbpSolution1.Permissions;
using AbpSolution1.Products;
using Microsoft.AspNetCore.Authorization;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Xunit;

namespace AbpSolution1.Application.Tests.Orders;

public class OrderAppServiceTests : AbpSolution1ApplicationTestBase<AbpSolution1ApplicationTestModule>
{
    private readonly OrderAppService _orderAppService;
    private readonly IRepository<Order, Guid> _orderRepository;
    private readonly IRepository<OrderDetails, Guid> _orderDetailsRepository;
    private readonly IRepository<Product, int> _productRepository;
    private readonly IRepository<Category, int> _categoryRepository;
    private readonly ICurrentUser _currentUser;

    public OrderAppServiceTests()
    {
        _orderAppService = GetRequiredService<OrderAppService>();
        _orderRepository = GetRequiredService<IRepository<Order, Guid>>();
        _orderDetailsRepository = GetRequiredService<IRepository<OrderDetails, Guid>>();
        _productRepository = GetRequiredService<IRepository<Product, int>>();
        _categoryRepository = GetRequiredService<IRepository<Category, int>>();
        _currentUser = GetRequiredService<ICurrentUser>();
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Create_Order_Successfully()
    {
        // Arrange
        Category category = null;
        Product product = null;

        await WithUnitOfWorkAsync(async () =>
        {
            // Create test category
            category = new Category { NameEn = "Electronics", NameAr = "الإلكترونيات" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            // Create test product
            product = new Product("Laptop", "Gaming Laptop", "لابتوب", "لابتوب للألعاب", 1000m, 10, category.Id);
            await _productRepository.InsertAsync(product, autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var createOrderDto = new CreateUpdateOrderDto
            {
                Details = new List<CreateUpdateOrderDetailsDto>
                {
                    new CreateUpdateOrderDetailsDto { ProductId = product.Id, Quantity = 2 }
                }
            };

            // Act
            var result = await _orderAppService.CreateOrderAsync(createOrderDto);

            // Assert
            result.ShouldBeTrue();
        });

        // Verify order was created
        await WithUnitOfWorkAsync(async () =>
        {
            var orders = await _orderRepository.GetListAsync();
            orders.ShouldNotBeEmpty();
            orders.First().OrderPrice.ShouldBe(2000m); // 1000 * 2
        });
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Decrease_Product_Stock()
    {
        // Arrange
        Category category = null;
        Product product = null;
        int initialStock = 50;

        await WithUnitOfWorkAsync(async () =>
        {
            category = new Category { NameEn = "Books", NameAr = "كتب" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product = new Product("Book", "Programming Book", "كتاب", "كتاب برمجة", 30m, initialStock, category.Id);
            await _productRepository.InsertAsync(product, autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var createOrderDto = new CreateUpdateOrderDto
            {
                Details = new List<CreateUpdateOrderDetailsDto>
                {
                    new CreateUpdateOrderDetailsDto { ProductId = product.Id, Quantity = 10 }
                }
            };

            // Act
            await _orderAppService.CreateOrderAsync(createOrderDto);
        });

        // Assert - Verify stock was decreased
        await WithUnitOfWorkAsync(async () =>
        {
            var updatedProduct = await _productRepository.GetAsync(product.Id);
            updatedProduct.Stock.ShouldBe(initialStock - 10);
        });
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Throw_Exception_When_Insufficient_Stock()
    {
        // Arrange
        Category category = null;
        Product product = null;

        await WithUnitOfWorkAsync(async () =>
        {
            category = new Category { NameEn = "Phones", NameAr = "هواتف" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product = new Product("Phone", "Smartphone", "هاتف", "هاتف ذكي", 500m, 5, category.Id);
            await _productRepository.InsertAsync(product, autoSave: true);
        });

        // Act & Assert
        await WithUnitOfWorkAsync(async () =>
        {
            var createOrderDto = new CreateUpdateOrderDto
            {
                Details = new List<CreateUpdateOrderDetailsDto>
                {
                    new CreateUpdateOrderDetailsDto { ProductId = product.Id, Quantity = 10 } // More than available
                }
            };

            await Should.ThrowAsync<BusinessException>(async () =>
            {
                await _orderAppService.CreateOrderAsync(createOrderDto);
            });
        });
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Create_Order_With_Multiple_Products()
    {
        // Arrange
        Category category = null;
        Product product1 = null;
        Product product2 = null;
        Product product3 = null;
        Guid orderId = Guid.Empty;

        await WithUnitOfWorkAsync(async () =>
        {
            category = new Category { NameEn = "Mixed", NameAr = "منتجات متنوعة" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product1 = new Product("Item1", "Description1", "منتج1", "وصف1", 100m, 50, category.Id);
            product2 = new Product("Item2", "Description2", "منتج2", "وصف2", 200m, 30, category.Id);
            product3 = new Product("Item3", "Description3", "منتج3", "وصف3", 150m, 20, category.Id);

            await _productRepository.InsertManyAsync(new[] { product1, product2, product3 }, autoSave: true);
            

            var createOrderDto = new CreateUpdateOrderDto
            {
                Details = new List<CreateUpdateOrderDetailsDto>
                {
                    new CreateUpdateOrderDetailsDto { ProductId = product1.Id, Quantity = 5 },
                    new CreateUpdateOrderDetailsDto { ProductId = product2.Id, Quantity = 3 },
                    new CreateUpdateOrderDetailsDto { ProductId = product3.Id, Quantity = 2 }
                }
            };

            // Act
            var result = await _orderAppService.CreateOrderAsync(createOrderDto);

            // Assert
            result.ShouldBeTrue();

            //// Get the created order
            var orders = await _orderRepository.GetListAsync();
            orders.ShouldNotBeEmpty();
            orderId = orders.First().Id;
        });

        // Verify order was created with correct total in a new unit of work
        await WithUnitOfWorkAsync(async () =>
        {
            var order = await _orderRepository.GetAsync(orderId);
            var expectedTotal = (100m * 5) + (200m * 3) + (150m * 2); // 500 + 600 + 300 = 1400
            order.OrderPrice.ShouldBe(expectedTotal);

            // Verify all order details were created
            var orderDetails = await _orderDetailsRepository.GetListAsync(od => od.OrderId == order.Id);
            orderDetails.Count.ShouldBe(3);
        });
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Throw_Exception_When_Product_Not_Found()
    {
        // Act & Assert
        await WithUnitOfWorkAsync(async () =>
        {
            var createOrderDto = new CreateUpdateOrderDto
            {
                Details = new List<CreateUpdateOrderDetailsDto>
                {
                    new CreateUpdateOrderDetailsDto { ProductId = 99999, Quantity = 1 } // Non-existent product
                }
            };

            await Should.ThrowAsync<BusinessException>(async () =>
            {
                await _orderAppService.CreateOrderAsync(createOrderDto);
            });
        });
    }

    [Fact]
    public async Task GetOrdersAsync_Should_Return_All_Orders_For_Admin()
    {
        // Arrange
        Category category = null;
        Product product = null;
        Guid order1Id = Guid.Empty;
        Guid order2Id = Guid.Empty;

        // Create test data
        await WithUnitOfWorkAsync(async () =>
        {
            category = new Category { NameEn = "Test", NameAr = "اختبار" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product = new Product("TestProduct", "Description", "منتج", "وصف", 100m, 100, category.Id);
            await _productRepository.InsertAsync(product, autoSave: true);

            // Use CreateOrderAsync to properly create orders
            var createOrderDto1 = new CreateUpdateOrderDto
            {
                Details = new List<CreateUpdateOrderDetailsDto>
                {
                    new CreateUpdateOrderDetailsDto { ProductId = product.Id, Quantity = 1 }
                }
            };

            var createOrderDto2 = new CreateUpdateOrderDto
            {
                Details = new List<CreateUpdateOrderDetailsDto>
                {
                    new CreateUpdateOrderDetailsDto { ProductId = product.Id, Quantity = 2 }
                }
            };

            await _orderAppService.CreateOrderAsync(createOrderDto1);
            await _orderAppService.CreateOrderAsync(createOrderDto2);
        });

        // Act & Assert
        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _orderAppService.GetOrdersAsync();

            // Verify orders were created
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThanOrEqualTo(2);
        });
    }

    [Fact]
    public async Task GetOrdersAsync_Should_Return_Orders_With_Product_Details()
    {
        // Arrange
        Category category = null;
        Product product1 = null;
        Product product2 = null;
        Guid orderId = Guid.Empty;

        await WithUnitOfWorkAsync(async () =>
        {
            category = new Category { NameEn = "Electronics", NameAr = "الإلكترونيات" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product1 = new Product("Mouse", "Gaming Mouse", "ماوس", "ماوس للألعاب", 50m, 20, category.Id);
            product2 = new Product("Keyboard", "Mechanical Keyboard", "لوحة مفاتيح", "لوحة مفاتيح ميكانيكية", 80m, 15, category.Id);

            await _productRepository.InsertManyAsync(new[] { product1, product2 }, autoSave: true);

            var createOrderDto = new CreateUpdateOrderDto
            {
                Details = new List<CreateUpdateOrderDetailsDto>
                {
                    new CreateUpdateOrderDetailsDto { ProductId = product1.Id, Quantity = 2 },
                    new CreateUpdateOrderDetailsDto { ProductId = product2.Id, Quantity = 1 }
                }
            };

            await _orderAppService.CreateOrderAsync(createOrderDto);

            // Get the created order ID
            var orders = await _orderRepository.GetListAsync();
            orderId = orders.First().Id;
        });

        await WithUnitOfWorkAsync(async () =>
        {
            // Act
            var result = await _orderAppService.GetOrdersAsync();

            // Assert
            result.ShouldNotBeEmpty();
            var orderDto = result.FirstOrDefault();
            orderDto.ShouldNotBeNull();
            orderDto.Details.Count.ShouldBe(2);
        });
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Set_Order_Date_Automatically()
    {
        // Arrange
        Category category = null;
        Product product = null;

        await WithUnitOfWorkAsync(async () =>
        {
            category = new Category { NameEn = "Test", NameAr = "اختبار" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product = new Product("TestProduct", "Description", "منتج", "وصف", 100m, 50, category.Id);
            await _productRepository.InsertAsync(product, autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var createOrderDto = new CreateUpdateOrderDto
            {
                Details = new List<CreateUpdateOrderDetailsDto>
                {
                    new CreateUpdateOrderDetailsDto { ProductId = product.Id, Quantity = 1 }
                }
            };

            var beforeCreate = DateTime.Now;

            // Act
            await _orderAppService.CreateOrderAsync(createOrderDto);

            var afterCreate = DateTime.Now;

            // Assert
            var orders = await _orderRepository.GetListAsync();
            var order = orders.First();
            order.OrderDate.ShouldBeGreaterThanOrEqualTo(beforeCreate);
            order.OrderDate.ShouldBeLessThanOrEqualTo(afterCreate);
        });
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Calculate_Correct_Total_Price()
    {
        // Arrange
        Category category = null;
        Product product = null;
        decimal expectedPrice = 1500m; // 500 * 3

        await WithUnitOfWorkAsync(async () =>
        {
            category = new Category { NameEn = "Accessories", NameAr = "إكسسوارات" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product = new Product("Headphones", "Wireless Headphones", "سماعات", "سماعات لاسلكية", 500m, 20, category.Id);
            await _productRepository.InsertAsync(product, autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var createOrderDto = new CreateUpdateOrderDto
            {
                Details = new List<CreateUpdateOrderDetailsDto>
                {
                    new CreateUpdateOrderDetailsDto { ProductId = product.Id, Quantity = 3 }
                }
            };

            // Act
            await _orderAppService.CreateOrderAsync(createOrderDto);

            // Assert
            var orders = await _orderRepository.GetListAsync();
            var order = orders.First();
            order.OrderPrice.ShouldBe(expectedPrice);
        });
    }
}
