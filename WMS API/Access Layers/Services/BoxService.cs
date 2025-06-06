using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Boxes;
using WMS_API.Models.Items;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class BoxService : IBoxService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IBoxRepository _boxRepository;
        private readonly IOrderRepository _orderRepository;
        private ControllerFunctions controllerFunctions;

        public BoxService(
            IItemRepository itemRepository,
            IBoxRepository boxRepository,
            IOrderRepository orderRepository
        )
        {
            _itemRepository = itemRepository;
            _boxRepository = boxRepository;
            _orderRepository = orderRepository;
            controllerFunctions = new ControllerFunctions();
        }

        public async Task<List<Box>> GetAllBoxesMostRecentDataAsync()
        {
            var result = await _boxRepository.GetAllBoxesMostRecentDataAsync();
            return result;
        }

        public async Task<Box> GetBoxByIdAsync(Guid boxId)
        {
            var result = await _boxRepository.GetBoxByIdAsync(boxId);
            return result;
        }

        public async Task<List<BoxData>> GetBoxHistoryByIdAsync(Guid boxId)
        {
            var result = await _boxRepository.GetBoxHistoryByIdAsync(boxId);
            return result;
        }

        public async Task RegisterBoxAsync(UnregisteredObject objectToRegister)
        {
            Guid boxId = Guid.NewGuid();

            BoxData newBoxData = new BoxData(
                DateTime.Now,
                objectToRegister.Name,
                objectToRegister.Description,
                objectToRegister.LengthInCentimeters,
                objectToRegister.WidthInCentimeters,
                objectToRegister.HeightInCentimeters,
                false,
                "Box Registered",
                boxId,
                null,
                null,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Box newBox = new Box(
                boxId,
                new List<BoxData>() { newBoxData },
                null
            );
            
            await _boxRepository.AddBoxAsync(newBox);
            controllerFunctions.printQrCode(objectToRegister.ObjectType + "-" + boxId);
        }

        public async Task<Order> PickItemIntoBoxAsync(Guid itemId, Guid boxId)
        {
            var itemDataToUpdate = await _itemRepository.GetItemDataByIdAsync(itemId);
            var boxDataToUpdate = await _boxRepository.GetBoxDataByIdAsync(boxId);

            if (itemDataToUpdate == null || boxDataToUpdate == null)
                return null;

            var dateTimeNow = DateTime.Now;

            Guid newItemDataEventId = Guid.NewGuid();
            itemDataToUpdate.NextEventId = newItemDataEventId;

            ItemData newItemData = new ItemData(
                dateTimeNow,
                itemDataToUpdate.Name,
                itemDataToUpdate.Description,
                itemDataToUpdate.LengthInCentimeters,
                itemDataToUpdate.WidthInCentimeters,
                itemDataToUpdate.HeightInCentimeters,
                itemDataToUpdate.WeightInKilograms,
                "Item Picked Into Box",
                itemDataToUpdate.ItemType,
                itemDataToUpdate.ItemId,
                null,
                itemDataToUpdate.OrderId,
                boxDataToUpdate.BoxId,
                newItemDataEventId,
                null,
                itemDataToUpdate.EventId
            );

            await _itemRepository.AddItemDataAsync(newItemData);
            return await _orderRepository.GetOrderByIdAsync((Guid)boxDataToUpdate.OrderId);
        }
    }
}
