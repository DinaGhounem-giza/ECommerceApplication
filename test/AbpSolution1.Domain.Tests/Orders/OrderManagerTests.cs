using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpSolution1.Categories;
using AbpSolution1.Products;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace AbpSolution1.Orders;

public class OrderManagerTests : AbpSolution1DomainTestBase<AbpSolution1DomainTestModule>
{
    private readonly OrderManager _orderManager;
    private readonly IRepository<Product, int> _productRepository;
    private readonly IRepository<Order, Guid> _orderRepository;
    private readonly IRepository<OrderDetails, Guid> _orderDetailsRepository;
    private readonly IRepository<Category, int> _categoryRepository;

    public OrderManagerTests()
    {
        _orderManager = GetRequiredService<OrderManager>();
        _productRepository = GetRequiredService<IRepository<Product, int>>();
        _orderRepository = GetRequiredService<IRepository<Order, Guid>>();
        _orderDetailsRepository = GetRequiredService<IRepository<OrderDetails, Guid>>();
        _categoryRepository = GetRequiredService<IRepository<Category, int>>();
    }

    [Fact]
    public async Task ProcessOrderCreationAsync_Should_Calculate_Correct_Total_Price()
    {
        // Arrange
        Category category = null;
        Product product1 = null;
        Product product2 = null;
        Order order = null;
        decimal expectedTotalPrice;

        await WithUnitOfWorkAsync(async () =>
        {
            // Create test category
            category = new Category { NameEn = "Electronics", NameAr = "الإلكترونيات" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            // Create test products
            product1 = new Product("Laptop", "Gaming Laptop", "لابتوب", "لابتوب للألعاب", 1000m, 10, category.Id);
            product2 = new Product("Mouse", "Gaming Mouse", "ماوس", "ماوس للألعاب", 50m, 20, category.Id);

            await _productRepository.InsertAsync(product1, autoSave: true);
            await _productRepository.InsertAsync(product2, autoSave: true);

            // Create order with details
            order = new Order();
            order.Details = new List<OrderDetails>
            {
                new OrderDetails(product1.Id, 2),
                new OrderDetails(product2.Id, 3)
            };

            expectedTotalPrice = (product1.Price * 2) + (product2.Price * 3); // (1000 * 2) + (50 * 3) = 2150

            // Act
            var actualTotalPrice = await _orderManager.ProcessOrderCreationAsync(order);

            // Assert
            actualTotalPrice.ShouldBe(expectedTotalPrice);
            order.OrderPrice.ShouldBe(expectedTotalPrice);
            order.OrderDate.ShouldNotBe(default(DateTime));
        });
    }

    [Fact]
    public async Task ProcessOrderCreationAsync_Should_Decrease_Product_Stock()
    {
        // Arrange
        Category category = null;
        Product product = null;
        Order order = null;
        int initialStock = 100;
        int orderQuantity = 10;

        await WithUnitOfWorkAsync(async () =>
        {
            // Create test data
            category = new Category { NameEn = "Books", NameAr = "كتب" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product = new Product("Book", "Programming Book", "كتاب", "كتاب برمجة", 30m, initialStock, category.Id);
            await _productRepository.InsertAsync(product, autoSave: true);

            order = new Order();
            order.Details = new List<OrderDetails>
            {
                new OrderDetails(product.Id, orderQuantity)
            };

            // Act
            await _orderManager.ProcessOrderCreationAsync(order);
        });

        // Assert - Verify stock was decreased
        await WithUnitOfWorkAsync(async () =>
        {
            var updatedProduct = await _productRepository.GetAsync(product.Id);
            updatedProduct.Stock.ShouldBe(initialStock - orderQuantity);
        });
    }

    [Fact]
    public async Task ProcessOrderCreationAsync_Should_Throw_Exception_When_Insufficient_Stock()
    {
        // Arrange
        Category category = null;
        Product product = null;
        Order order = null;

        await WithUnitOfWorkAsync(async () =>
        {
            category = new Category { NameEn = "Phones", NameAr = "هواتف" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product = new Product("Phone", "Smartphone", "هاتف", "هاتف ذكي", 500m, 5, category.Id);
            await _productRepository.InsertAsync(product, autoSave: true);

            order = new Order();
            order.Details = new List<OrderDetails>
            {
                new OrderDetails(product.Id, 10) // More than available stock
            };

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(async () =>
            {
                await _orderManager.ProcessOrderCreationAsync(order);
            });

            exception.Message.Equals("Stock is less than the required quantity");

        });
    }

    [Fact]
    public async Task ProcessOrderCreationAsync_Should_Throw_Exception_When_Product_Not_Found()
    {
        // Arrange
        var order = new Order();
        order.Details = new List<OrderDetails>
        {
            new OrderDetails(99999, 1) // Non-existent product
        };

        // Act & Assert
        await WithUnitOfWorkAsync(async () =>
        {
            var exception = await Should.ThrowAsync<BusinessException>(async () =>
            {
                await _orderManager.ProcessOrderCreationAsync(order);
            });

            exception.Message.Contains("Product Not Found");
        });
    }

    [Fact]
    public async Task ProcessOrderCreationAsync_Should_Throw_Exception_When_Order_Has_No_Items()
    {
        // Arrange
        var order = new Order();
        order.Details = new List<OrderDetails>(); // Empty order

        // Act & Assert
        await WithUnitOfWorkAsync(async () =>
        {
            var exception = await Should.ThrowAsync<BusinessException>(async () =>
            {
                await _orderManager.ProcessOrderCreationAsync(order);
            });

            exception.Message.Contains("Order must have at least one item.");
        });
    }

    [Fact]
    public async Task SaveOrder_Should_Persist_Order_And_OrderDetails()
    {
        // Arrange
        Category category = null;
        Product product = null;
        Order order = null;
        Guid orderId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            // Create test data
            category = new Category { NameEn = "Accessories", NameAr = "إكسسوارات" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product = new Product("Keyboard", "Mechanical Keyboard", "لوحة مفاتيح", "لوحة مفاتيح ميكانيكية", 80m, 15, category.Id);
            await _productRepository.InsertAsync(product, autoSave: true);

            order = new Order();
            order.Details = new List<OrderDetails>
            {
                new OrderDetails(product.Id, 2)
            };

            await _orderManager.ProcessOrderCreationAsync(order);
            orderId = order.Id;
            

            // Act
            await _orderManager.SaveOrder(order);
        });

        // Assert
        await WithUnitOfWorkAsync(async () =>
        {
            var savedOrder = await _orderRepository.GetAsync(orderId);
            savedOrder.ShouldNotBeNull();
            savedOrder.OrderPrice.ShouldBe(160m); // 80 * 2

            var orderDetails = await _orderDetailsRepository.GetListAsync(od => od.OrderId == orderId);
            orderDetails.Count.ShouldBe(1);
            orderDetails.First().Quantity.ShouldBe(2);
            orderDetails.First().ProductId.ShouldBe(product.Id);
        });
    }

    [Fact]
    public async Task ProcessOrderCreationAsync_Should_Handle_Multiple_Products_Correctly()
    {
        // Arrange
        Category category = null;
        Product product1 = null;
        Product product2 = null;
        Product product3 = null;
        Order order = null;

        await WithUnitOfWorkAsync(async () =>
        {
            category = new Category { NameEn = "Mixed", NameAr = "منتجات متنوعة" };
            await _categoryRepository.InsertAsync(category, autoSave: true);

            product1 = new Product("Item1", "Description1", "منتج1", "وصف1", 100m, 50, category.Id);
            product2 = new Product("Item2", "Description2", "منتج2", "وصف2", 200m, 30, category.Id);
            product3 = new Product("Item3", "Description3", "منتج3", "وصف3", 150m, 20, category.Id);

            await _productRepository.InsertAsync(product1, autoSave: true);
            await _productRepository.InsertAsync(product2, autoSave: true);
            await _productRepository.InsertAsync(product3, autoSave: true);

            order = new Order();
            order.Details = new List<OrderDetails>
            {
                new OrderDetails(product1.Id, 5),
                new OrderDetails(product2.Id, 3),
                new OrderDetails(product3.Id, 2)
            };

            // Act
            var totalPrice = await _orderManager.ProcessOrderCreationAsync(order);

            // Assert
            var expectedTotal = (100m * 5) + (200m * 3) + (150m * 2); // 500 + 600 + 300 = 1400
            totalPrice.ShouldBe(expectedTotal);
            order.OrderPrice.ShouldBe(expectedTotal);
        });

        // Verify all stocks were decreased
        await WithUnitOfWorkAsync(async () =>
        {
            var updatedProduct1 = await _productRepository.GetAsync(product1.Id);
            var updatedProduct2 = await _productRepository.GetAsync(product2.Id);
            var updatedProduct3 = await _productRepository.GetAsync(product3.Id);

            updatedProduct1.Stock.ShouldBe(45); // 50 - 5
            updatedProduct2.Stock.ShouldBe(27); // 30 - 3
            updatedProduct3.Stock.ShouldBe(18); // 20 - 2
        });
    }
}
