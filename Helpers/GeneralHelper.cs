using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
