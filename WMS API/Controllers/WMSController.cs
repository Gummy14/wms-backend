using Microsoft.AspNetCore.Mvc;
using ZXing;
using ZXing.Common;
using System.Drawing;
using ZXing.Windows.Compatibility;
using System.Drawing.Printing;
using System.Text.Json;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Boxes;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;
using ContainerData = WMS_API.Models.Containers.ContainerData;
using WMS_API.Models.Containers;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WMSController : ControllerBase
    {
        private MyDbContext dBContext;

        public WMSController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpPost("PrintQRCode")]
        public async Task<StatusCodeResult> PrintQRCode(UnregisteredObject objectToRegister)
        {
            Guid objectId = Guid.NewGuid();
            objectToRegister.Id = objectId;

            await RegisterWarehouseObject(objectToRegister);

            printQrCodeFromRegistrationString(objectToRegister.ObjectType.ToString() + '-' + objectToRegister.Id.ToString());

            return StatusCode(200);
        }

        [HttpPost("RegisterQRCode")]
        public async Task<StatusCodeResult> RegisterQRCode(UnregisteredObject objectToRegister)
        {
            await RegisterWarehouseObject(objectToRegister);

            return StatusCode(200);
        }

        private void printQrCodeFromRegistrationString(string registrationString)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 200,
                    Width = 200,
                    Margin = 1
                }
            };

            var result = writer.Write(registrationString);
            var barcodeBitmap = new Bitmap(result);

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                Graphics graphics = e.Graphics;
                RectangleF printArea = e.MarginBounds;
                graphics.DrawImage(barcodeBitmap, printArea);
            };

            printDocument.Print();
        }

        private async Task RegisterWarehouseObject(UnregisteredObject objectToRegister)
        {
            switch (objectToRegister.ObjectType)
            {
                case 0:
                    RegisterItem(objectToRegister);
                    break;
                case 1:
                    RegisterLocation(objectToRegister);
                    break;
                case 2:
                    RegisterContainer(objectToRegister);
                    break;
                //case 3:
                //    RegisterOrder(objectToRegister);
                //    break;
                case 4:
                    RegisterBox(objectToRegister);
                    break;
                default:
                    break;
            }

            await dBContext.SaveChangesAsync();
        }

        private void RegisterItem(UnregisteredObject objectToRegister)
        {
            
            ItemData newItemData = new ItemData(
                DateTime.Now,
                Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY,
                objectToRegister.Name,
                objectToRegister.Description,
                objectToRegister.LengthInCentimeters,
                objectToRegister.WidthInCentimeters,
                objectToRegister.HeightInCentimeters,
                objectToRegister.WeightOrMaxWeightInKilograms,
                (Guid)objectToRegister.Id,
                null,
                null,
                null,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Item newItem = new Item(
                (Guid)objectToRegister.Id,
                new List<ItemData>() { newItemData },
                null
            );

            dBContext.Items.Add(newItem);
        }

        private void RegisterLocation(UnregisteredObject objectToRegister)
        {
            LocationData newLocationData = new LocationData(
                DateTime.Now,
                Constants.LOCATION_UNOCCUPIED,
                objectToRegister.Name,
                objectToRegister.Description,
                objectToRegister.LengthInCentimeters,
                objectToRegister.WidthInCentimeters,
                objectToRegister.HeightInCentimeters,
                objectToRegister.WeightOrMaxWeightInKilograms,
                (Guid)objectToRegister.Id,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Location newLocation = new Location(
                (Guid)objectToRegister.Id,
                new List<LocationData>() { newLocationData },
                null
            );

            dBContext.Locations.Add(newLocation);
        }

        private void RegisterContainer(UnregisteredObject objectToRegister)
        {
            ContainerData newContainerData = new ContainerData(
                DateTime.Now,
                Constants.CONTAINER_NOT_IN_USE,
                objectToRegister.Name,
                objectToRegister.Description,
                (Guid)objectToRegister.Id,
                Guid.NewGuid(),
                null,
                null
            );

            Container newContainer = new Container(
                (Guid)objectToRegister.Id,
                new List<ContainerData>() { newContainerData },
                null
            );

            dBContext.Containers.Add(newContainer);
        }

        private void RegisterBox(UnregisteredObject objectToRegister)
        {
            
            BoxData newBoxData = new BoxData(
                DateTime.Now,
                Constants.BOX_REGISTERED,
                objectToRegister.Name,
                objectToRegister.Description,
                objectToRegister.LengthInCentimeters,
                objectToRegister.WidthInCentimeters,
                objectToRegister.HeightInCentimeters,
                (Guid)objectToRegister.Id,
                Guid.NewGuid(),
                null,
                null
            );

            Box newBox = new Box(
                (Guid)objectToRegister.Id,
                new List<BoxData>() { newBoxData },
                null
            );

            dBContext.Boxes.Add(newBox);
        }
    }
}
