using ZXing;
using ZXing.Common;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;
using System.Text.Json;
using System.Drawing.Printing;

namespace WMS_API.Controllers
{
    public class ControllerFunctions
    {
        public ControllerFunctions()
        {
        }

        public void printQrCodeFromRegistrationString(string registrationString)
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
    }
}
