using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpSolution1.Products;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AbpSolution1.Orders
{
    public class OrderManager : DomainService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<OrderDetails, Guid> _orderDetailsRepository;

        public OrderManager(IRepository<Product, int> productRepository,IRepository<Order, Guid> orderRepository,
        IRepository<OrderDetails, Guid> orderDetailsRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
        }

        private async Task ValidateOrderStockAsync(List<OrderDetails> orderDetails)
        {
            var productIds = orderDetails.Select(od => od.ProductId).Distinct().ToList();
            var products = await _productRepository.GetListAsync(p => productIds.Contains(p.Id));

            foreach (var orderDetail in orderDetails)
            {
                var product = products.FirstOrDefault(p => p.Id == orderDetail.ProductId);

                if (product == null)
                {
                    throw new BusinessException("Product Not Found");
                }

                if (product.Stock < orderDetail.Quantity)
                {
                    throw new BusinessException("Stock is less than the required quantity");
                }
            }
        }

      
        private async Task<decimal> CalculateOrderPriceAsync(List<OrderDetails> orderDetails)
        {
            var productIds = orderDetails.Select(od => od.ProductId).Distinct().ToList();
            var products = await _productRepository.GetListAsync(p => productIds.Contains(p.Id));

            decimal totalPrice = 0;

            foreach (var orderDetail in orderDetails)
            {
                var product = products.FirstOrDefault(p => p.Id == orderDetail.ProductId);

                totalPrice += product.Price * orderDetail.Quantity;
            }

            return totalPrice;
        }

       
        private async Task DecreaseStockAsync(List<OrderDetails> orderDetails)
        {
            var productIds = orderDetails.Select(od => od.ProductId).Distinct().ToList();
            var products = await _productRepository.GetListAsync(p => productIds.Contains(p.Id));

            foreach (var orderDetail in orderDetails)
            {
                var product = products.FirstOrDefault(p => p.Id == orderDetail.ProductId);

                product.SetStock(product.Stock - orderDetail.Quantity);
            }

            await _productRepository.UpdateManyAsync(products);
        }

        public async Task<decimal> ProcessOrderCreationAsync(Order order)
        {

            if (order.Details == null || !order.Details.Any())
            {
                throw new BusinessException("Order must have at least one item.");
            }

            await ValidateOrderStockAsync(order.Details);

            var totalPrice = await CalculateOrderPriceAsync(order.Details);
            order.OrderPrice = totalPrice;
            order.OrderDate = DateTime.Now;

            await DecreaseStockAsync(order.Details);

            return totalPrice;
        }
    
        public async Task SaveOrder(Order order)
        {
            await _orderRepository.InsertAsync(order);

            order.Details.ForEach(details => details.OrderId = order.Id);

            await _orderDetailsRepository.InsertManyAsync(order.Details, autoSave:true);
        }
    }
}
