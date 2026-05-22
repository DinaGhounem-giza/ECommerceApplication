using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpSolution1.DTOs.Orders;
using AbpSolution1.Orders;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

public class OrderAppService : ApplicationService
{
    private readonly IRepository<Order, Guid> _orderRepository;
    private readonly IRepository<OrderDetails, Guid> _orderDetailsRepository;
    private readonly OrderManager _orderManager;

    public OrderAppService(IRepository<Order, Guid> orderRepository,
        IRepository<OrderDetails, Guid> orderDetailsRepository, OrderManager orderManager)
    {
        _orderRepository = orderRepository;
        _orderDetailsRepository = orderDetailsRepository;
        _orderManager = orderManager;
    }

    public async Task<OrderDto> GetOrderAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);
        var orderDetails = await _orderDetailsRepository.GetListAsync(od => od.OrderId == id);
        var orderDto = ObjectMapper.Map<Order, OrderDto>(order);
        orderDto.Details = orderDetails.Select(od => ObjectMapper.Map<OrderDetails, OrderDetailsDto>(od)).ToList();
        return orderDto;
    }

    public async Task<List<OrderDto>> GetOrdersAsync()
    {
        var orders = await _orderRepository.GetListAsync();
        var orderDtos = new List<OrderDto>();
        foreach (var order in orders)
        {
            var orderDetails = await _orderDetailsRepository.GetListAsync(od => od.OrderId == order.Id);
            var orderDto = ObjectMapper.Map<Order, OrderDto>(order);
            orderDto.Details = orderDetails.Select(od => ObjectMapper.Map<OrderDetails, OrderDetailsDto>(od)).ToList();
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