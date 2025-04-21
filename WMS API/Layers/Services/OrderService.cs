using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private ControllerFunctions controllerFunctions;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            controllerFunctions = new ControllerFunctions();
        }

        public async Task<List<OrderData>> GetAllOrdersAsync()
        {
            var result = await _orderRepository.GetAllOrdersAsync();
            return result;
        }

        public async Task<OrderData> GetOrderByIdAsync(Guid orderId)
        {
            var result = await _orderRepository.GetOrderByIdAsync(orderId);
            return result;
        }

        public async Task<List<OrderData>> GetOrderHistoryAsync(Guid orderId)
        {
            var result = await _orderRepository.GetOrderHistoryAsync(orderId);
            return result;
        }

        public async Task RegisterOrderAsync(UnregisteredObject objectToRegister)
        {
            throw new NotImplementedException();
        }
    }
}
