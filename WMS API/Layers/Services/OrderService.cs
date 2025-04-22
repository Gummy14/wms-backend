using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Items;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private ControllerFunctions controllerFunctions;

        public OrderService(IOrderRepository orderRepository, IItemRepository itemRepository)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
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

        public async Task RegisterOrderAsync(UnregisteredOrder unregisteredOrder)
        {
            var itemDataToUpdateNextEventIdOn = await _itemRepository.GetAllItemsInOrderAsync(unregisteredOrder.OrderItems);

            Guid orderId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            string orderName = Math.Floor(DateTime.Now.Subtract(new DateTime(2020, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string orderDescription = "Order Containing: ";
            List<ItemData> newOrderItemData = new List<ItemData>();

            int counter = 0;
            foreach (ItemData itemData in itemDataToUpdateNextEventIdOn)
            {
                Guid newItemDataEventId = Guid.NewGuid();
                itemData.NextEventId = newItemDataEventId;

                ItemData newItemData = new ItemData(
                    dateTimeNow,
                    itemData.Name,
                    itemData.Description,
                    itemData.LengthInCentimeters,
                    itemData.WidthInCentimeters,
                    itemData.HeightInCentimeters,
                    itemData.WeightInKilograms,
                    itemData.ItemId,
                    itemData.LocationId,
                    itemData.ContainerId,
                    orderId,
                    itemData.BoxId,
                    newItemDataEventId,
                    null,
                    itemData.EventId
                );

                if (counter == itemDataToUpdateNextEventIdOn.Count() - 1)
                {
                    orderDescription += itemData.Name;
                }
                else
                {
                    orderDescription += itemData.Name + ", ";
                }
                counter++;

                newOrderItemData.Add(newItemData);
            }

            Address newOrderAddress = new Address(
                Guid.NewGuid(),
                orderId,
                unregisteredOrder.Address.FirstName,
                unregisteredOrder.Address.LastName,
                unregisteredOrder.Address.Street,
                unregisteredOrder.Address.City,
                unregisteredOrder.Address.State,
                unregisteredOrder.Address.Zip
            );

            OrderData newOrderData = new OrderData(
                DateTime.Now,
                orderName,
                orderDescription,
                orderId,
                Guid.NewGuid(),
                null,
                null
            );

            Order newOrder = new Order(
                orderId,
                new List<OrderData>() { newOrderData },
                newOrderItemData,
                newOrderAddress,
                null,
                null
            );
            await _orderRepository.AddOrderAsync(newOrder);
        }
    }
}
