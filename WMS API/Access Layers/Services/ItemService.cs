﻿using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Items;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private ControllerFunctions controllerFunctions;

        public ItemService(
            IItemRepository itemRepository
        )
        {
            _itemRepository = itemRepository;
            controllerFunctions = new ControllerFunctions();
        }

        public async Task<List<Item>> GetAllItemsMostRecentDataAsync()
        {
            var result = await _itemRepository.GetAllItemsMostRecentDataAsync();
            return result;
        }
        
        public async Task<Item> GetItemByIdAsync(Guid itemId)
        {
            var result = await _itemRepository.GetItemByIdAsync(itemId);
            return result;
        }
        
        public async Task<List<ItemData>> GetItemHistoryByIdAsync(Guid itemId)
        {
            var result = await _itemRepository.GetItemHistoryByIdAsync(itemId);
            return result;
        }

        public async Task RegisterItemAsync(UnregisteredObject objectToRegister)
        {
            Guid itemId = Guid.NewGuid();

            ItemData newItemData = new ItemData(
                DateTime.Now,
                objectToRegister.Name,
                objectToRegister.Description,
                objectToRegister.LengthInCentimeters,
                objectToRegister.WidthInCentimeters,
                objectToRegister.HeightInCentimeters,
                objectToRegister.WeightOrMaxWeightInKilograms,
                "Item Registered",
                Guid.NewGuid(),
                itemId,
                null,
                null,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Item newItem = new Item(
                itemId,
                new List<ItemData>() { newItemData }
            );

            await _itemRepository.AddItemAsync(newItem);
            controllerFunctions.printQrCode(objectToRegister.ObjectType + "-" + itemId);
        }
    }
}
