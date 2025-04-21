using Microsoft.AspNetCore.Mvc;
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
    public class ContainerService : IContainerService
    {
        private readonly IContainerRepository _containerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IBoxRepository _boxRepository;
        private ControllerFunctions controllerFunctions;

        public ContainerService(
            IContainerRepository containerRepository, 
            IOrderRepository orderRepository,
            IBoxRepository boxRepository
        )
        {
            _containerRepository = containerRepository;
            _orderRepository = orderRepository;
            _boxRepository = boxRepository;
            controllerFunctions = new ControllerFunctions();
        }

        public async Task<List<ContainerData>> GetAllContainersAsync()
        {
            var result = await _containerRepository.GetAllContainersAsync();
            return result;
        }

        public async Task<ContainerData> GetContainerByIdAsync(Guid containerId)
        {
            var result = await _containerRepository.GetContainerByIdAsync(containerId);
            return result;
        }

        public async Task<List<ContainerData>> GetContainerHistoryAsync(Guid containerId)
        {
            var result = await _containerRepository.GetContainerHistoryAsync(containerId);
            return result;
        }

        public async Task RegisterContainerAsync(UnregisteredObject objectToRegister)
        {
            Guid containerId = Guid.NewGuid();

            ContainerData newContainerData = new ContainerData(
                DateTime.Now,
                objectToRegister.Name,
                objectToRegister.Description,
                containerId,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Container newContainer = new Container(
                containerId,
                new List<ContainerData>() { newContainerData },
                null
            );

            await _containerRepository.AddContainerAsync(newContainer);
            controllerFunctions.printQrCode(objectToRegister.ObjectType + "-" + containerId);
        }
        
        public async Task AddContainerToOrderAsync(Guid orderId, Guid containerId)
        {
            var containerDataToUpdate = await _containerRepository.GetContainerByIdAsync(containerId);
            var orderDataToUpdate = await _orderRepository.GetOrderByIdAsync(orderId);

            if (containerDataToUpdate != null && orderDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newContainerDataEventId = Guid.NewGuid();
                containerDataToUpdate.NextEventId = newContainerDataEventId;

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    containerDataToUpdate.ContainerId,
                    orderDataToUpdate.OrderId,
                    newContainerDataEventId,
                    null,
                    containerDataToUpdate.EventId
                );

                await _containerRepository.AddContainerDataAsync(newContainerData);
            }
        }

        public async Task RemoveContainerFromOrderAsync(Guid containerId)
        {
            var containerDataToUpdate = await _containerRepository.GetContainerByIdAsync(containerId);
            var boxDataToUpdate = await _boxRepository.GetBoxByIdAsync((Guid)containerDataToUpdate.OrderId);

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
