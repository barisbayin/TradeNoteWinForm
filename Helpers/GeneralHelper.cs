using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace TradeNote.Helpers
{
    public static class GeneralHelper
    {
        public static string GetXmlFilePath(string xmlFileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string subDirectory = Path.Combine(currentDirectory, "TradeNotes");

            if (!Directory.Exists(subDirectory))
            {
                Directory.CreateDirectory(subDirectory);
            }

            string xmlFilePath = Path.Combine(subDirectory, xmlFileName + ".xml");

            return xmlFilePath;

        }

        public static string GetTradeNotesDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string subDirectory = Path.Combine(currentDirectory, "TradeNotes");


            if (!Directory.Exists(subDirectory))
            {
                Directory.CreateDirectory(subDirectory);
            }
            return subDirectory;
        }

        public static Image GenerateQrCodeImageByGivenString(string text)
        {
            var qrCodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.QrCode.QrCodeEncodingOptions
                {
                    Width = 120,
                    Height = 120,
                    Margin = 1
                }
            };
            var qrCode = qrCodeWriter.Write(text);
            qrCode.Save("qrcode.png", ImageFormat.Png);

            return qrCode;
        }


    }
}
