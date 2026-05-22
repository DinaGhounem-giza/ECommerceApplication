using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpSolution1.DTOs.Orders;
using AbpSolution1.Orders;
using AbpSolution1.Permissions;
using AbpSolution1.Products;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

public class OrderAppService : ApplicationService
{
    private readonly IRepository<Order, Guid> _orderRepository;
    private readonly IRepository<OrderDetails, Guid> _orderDetailsRepository;
    private readonly IRepository<Product, int> _productRepository;
    private readonly OrderManager _orderManager;

    public OrderAppService(IRepository<Order, Guid> orderRepository,
        IRepository<OrderDetails, Guid> orderDetailsRepository,
        IRepository<Product,int> productRepository,
        OrderManager orderManager)
    {
        _orderRepository = orderRepository;
        _orderDetailsRepository = orderDetailsRepository;
        _productRepository = productRepository;
        _orderManager = orderManager;
    }

    public async Task<List<OrderDto>> GetOrdersAsync()
    {
        var orders = await _orderRepository.GetListAsync();
        var orderDtos = new List<OrderDto>();
        foreach (var order in orders)
        {
            var orderDetails = await _orderDetailsRepository.GetListAsync(od => od.OrderId == order.Id);
            var orderDto = new OrderDto
            {
                OrderDate = order.OrderDate,
                OrderPrice = order.OrderPrice
            };

            List<OrderDetailsDto> detailsDtos = new List<OrderDetailsDto>();

            foreach (var od in orderDetails)
            {
                var product = await _productRepository.GetAsync(od.ProductId);
                var detailsDto = new OrderDetailsDto
                {
                    ProductNameAr = product.NameAr,
                    ProductNameEn = product.NameEn,
                    Quantity = od.Quantity
                };
                detailsDtos.Add(detailsDto);
            }

            orderDto.Details = detailsDtos;
            orderDtos.Add(orderDto);
        }
        return orderDtos;
    }

    public async Task<bool> CreateOrderAsync(CreateUpdateOrderDto input)
    {
        var order = new Order
        {
            Details = input.Details.Select(item =>
                new OrderDetails(item.ProductId, item.Quantity)).ToList()
        };

        await _orderManager.ProcessOrderCreationAsync(order);

        await _orderManager.SaveOrder(order);

        return true;
    }
}