using ZXing;
using ZXing.Common;
using System.Drawing;
using ZXing.Windows.Compatibility;
using System.Drawing.Printing;
using WMS_API.Models.Orders;

namespace WMS_API.Controllers
{
    public class ControllerFunctions
    {
        public ControllerFunctions()
        {
        }

        public void printQrCode(string registrationString)
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

        public void printShippingLabel(Address address)
        {
            string addressString = $"{address.FirstName} {address.LastName}\n" +
                $"{address.Street}\n" +
                $"{address.City}, {address.State} {address.Zip}";

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                Graphics graphics = e.Graphics;
                RectangleF printArea = e.MarginBounds;
                Font printFont = new Font("Arial", 12);
                Brush printBrush = Brushes.Black;
                graphics.DrawString(addressString, printFont, printBrush, printArea);
            };

            printDocument.Print();
        }
    }
}
