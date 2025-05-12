using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Boxes;
using WMS_API.Models.Containers;
using WMS_API.Models.Items;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IContainerRepository _containerRepository;
        private readonly IBoxRepository _boxRepository;

        public OrderService(
            IOrderRepository orderRepository, 
            IItemRepository itemRepository,
            IContainerRepository containerRepository,
            IBoxRepository boxRepository
        )
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _containerRepository = containerRepository;
            _boxRepository = boxRepository;
        }

        public async Task<List<Order>> GetAllOrdersMostRecentDataAsync()
        {
            var result = await _orderRepository.GetAllOrdersMostRecentDataAsync();
            return result;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            var result = await _orderRepository.GetOrderByIdAsync(orderId);
            return result;
        }

        public async Task<List<OrderData>> GetOrderHistoryByIdAsync(Guid orderId)
        {
            var result = await _orderRepository.GetOrderHistoryByIdAsync(orderId);
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
                    "Item Added To Order",
                    itemData.ItemType,
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
                "Order Registered",
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
        
        public async Task<Order> AddContainerToOrderAsync(Guid orderId, Guid containerId)
        {
            var containerDataToUpdate = await _containerRepository.GetContainerDataByIdAsync(containerId);
            var orderToAddContainerTo = await _orderRepository.GetOrderDataByIdAsync(orderId);

            if (containerDataToUpdate == null || orderToAddContainerTo == null)
                return null;

            var dateTimeNow = DateTime.Now;

            Guid newContainerDataEventId = Guid.NewGuid();
            containerDataToUpdate.NextEventId = newContainerDataEventId;

            ContainerData newContainerData = new ContainerData(
                dateTimeNow,
                containerDataToUpdate.Name,
                containerDataToUpdate.Description,
                "Container Added To Order",
                containerDataToUpdate.ContainerId,
                orderToAddContainerTo.OrderId,
                newContainerDataEventId,
                null,
                containerDataToUpdate.EventId
            );

            await _containerRepository.AddContainerDataAsync(newContainerData);
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<Order> AddBoxToOrderAsync(Guid orderId, Guid boxId)
        {
            var orderDataToUpdate = await _orderRepository.GetOrderDataByIdAsync(orderId);
            var boxDataToUpdate = await _boxRepository.GetBoxDataByIdAsync(boxId);

            if (boxDataToUpdate == null || orderDataToUpdate == null)
                return null;

            var dateTimeNow = DateTime.Now;

            Guid newBoxDataEventId = Guid.NewGuid();
            boxDataToUpdate.NextEventId = newBoxDataEventId;

            BoxData newBoxData = new BoxData(
                dateTimeNow,
                boxDataToUpdate.Name,
                boxDataToUpdate.Description,
                boxDataToUpdate.LengthInCentimeters,
                boxDataToUpdate.WidthInCentimeters,
                boxDataToUpdate.HeightInCentimeters,
                boxDataToUpdate.IsSealed,
                "Box Added To Order",
                boxDataToUpdate.BoxId,
                boxDataToUpdate.ShipmentId,
                boxDataToUpdate.TruckId,
                orderDataToUpdate.OrderId,
                newBoxDataEventId,
                null,
                boxDataToUpdate.EventId
            );

            await _boxRepository.AddBoxDataAsync(newBoxData);
            return await _orderRepository.GetOrderByIdAsync(orderDataToUpdate.OrderId);
        }

        public async Task RemoveContainerFromOrderAsync(Guid containerId)
        {
            var containerDataToUpdate = await _containerRepository.GetContainerDataByIdAsync(containerId);
            var boxDataToUpdate = await _boxRepository.GetBoxDataByIdAsync((Guid)containerDataToUpdate.OrderId);

            if (containerDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newContainerDataEventId = Guid.NewGuid();
                containerDataToUpdate.NextEventId = newContainerDataEventId;

                Guid newBoxDataEventId = Guid.NewGuid();
                boxDataToUpdate.NextEventId = newBoxDataEventId;

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    "Container Removed From Order",
                    containerDataToUpdate.ContainerId,
                    null,
                    newContainerDataEventId,
                    null,
                    containerDataToUpdate.EventId
                );

                BoxData newBoxData = new BoxData(
                    dateTimeNow,
                    boxDataToUpdate.Name,
                    boxDataToUpdate.Description,
                    boxDataToUpdate.LengthInCentimeters,
                    boxDataToUpdate.WidthInCentimeters,
                    boxDataToUpdate.HeightInCentimeters,
                    true,
                    "Box Sealed",
                    boxDataToUpdate.BoxId,
                    boxDataToUpdate.ShipmentId,
                    boxDataToUpdate.TruckId,
                    boxDataToUpdate.OrderId,
                    newBoxDataEventId,
                    null,
                boxDataToUpdate.EventId
                );

                await _containerRepository.AddContainerDataAsync(newContainerData);
                await _boxRepository.AddBoxDataAsync(boxDataToUpdate);
            }
        }
    }
}
