using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Boxes;
using WMS_API.Models.Shipments;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class BoxService : IBoxService
    {
        private readonly IBoxRepository _boxRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ITruckRepository _truckRepository;
        private ControllerFunctions controllerFunctions;

        public BoxService(
            IBoxRepository boxRepository, 
            IOrderRepository orderRepository,
            IShipmentRepository shipmentRepository,
            IAddressRepository addressRepository,
            ITruckRepository truckRepository
        )
        {
            _boxRepository = boxRepository;
            _orderRepository = orderRepository;
            _shipmentRepository = shipmentRepository;
            _addressRepository = addressRepository;
            _truckRepository = truckRepository;
            controllerFunctions = new ControllerFunctions();
        }

        public async Task<List<BoxData>> GetAllBoxesAsync()
        {
            var result = await _boxRepository.GetAllBoxesAsync();
            return result;
        }

        public async Task<BoxData> GetBoxByIdAsync(Guid boxId)
        {
            var result = await _boxRepository.GetBoxByIdAsync(boxId);
            return result;
        }

        public async Task<List<BoxData>> GetBoxHistoryAsync(Guid boxId)
        {
            var result = await _boxRepository.GetBoxHistoryAsync(boxId);
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

        public async Task AddBoxToOrderAsync(Guid orderId, Guid boxId)
        {
            var orderDataToUpdate = await _orderRepository.GetOrderByIdAsync(orderId);
            var boxDataToUpdate = await _boxRepository.GetBoxByIdAsync(boxId);

            if (boxDataToUpdate != null && orderDataToUpdate != null)
            {
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
                    boxDataToUpdate.BoxId,
                    boxDataToUpdate.ShipmentId,
                    boxDataToUpdate.TruckId,
                    orderDataToUpdate.OrderId,
                    newBoxDataEventId,
                    null,
                    boxDataToUpdate.EventId
                );

                await _boxRepository.AddBoxDataAsync(newBoxData);
            }

        }

        public async Task AddBoxToShipmentAsync(Guid boxId)
        {
            var boxDataToUpdate = await _boxRepository.GetBoxByIdAsync(boxId);
            var shipmentData = await _shipmentRepository.GetNextShipmentAsync();
            var addressToPrint = await _addressRepository.GetAddressByOrderIdAsync((Guid)boxDataToUpdate.OrderId);

            if (boxDataToUpdate != null && shipmentData != null)
            {
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
                    boxDataToUpdate.BoxId,
                    shipmentData.ShipmentId,
                    boxDataToUpdate.TruckId,
                    boxDataToUpdate.OrderId,
                    newBoxDataEventId,
                    null,
                    boxDataToUpdate.EventId
                );

                await _boxRepository.AddBoxDataAsync(newBoxData);
                controllerFunctions.printShippingLabel(addressToPrint);
            }
        }

        public async Task AddBoxToTruck(Guid boxId, Guid truckId)
        {
            var boxDataToUpdate = await _boxRepository.GetBoxByIdAsync(boxId);
            var truckData = await _truckRepository.GetTruckByIdAsync(truckId);

            if (boxDataToUpdate != null && truckData != null)
            {
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
                    boxDataToUpdate.BoxId,
                    boxDataToUpdate.ShipmentId,
                    truckData.Id,
                    boxDataToUpdate.OrderId,
                    newBoxDataEventId,
                    null,
                    boxDataToUpdate.EventId
                );

                await _boxRepository.AddBoxDataAsync(newBoxData);
            }
        }
    }
}
