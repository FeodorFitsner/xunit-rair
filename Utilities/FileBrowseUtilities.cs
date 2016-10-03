using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Rair.Utilities.Windows.Utilities
{
    public class FileBrowseUtilities
    {
        public static FileInfo BrowseForFile(string filter, Environment.SpecialFolder initialDirectory = Environment.SpecialFolder.MyDocuments)
        {
            var dialog = new OpenFileDialog
            {
                Filter = filter,
                InitialDirectory = Environment.GetFolderPath(initialDirectory)
            };
            return dialog.ShowDialog() == false ? null : new FileInfo(dialog.FileName);
        }

        public static FileInfo SaveFile(string filter, Environment.SpecialFolder initialDirectory = Environment.SpecialFolder.MyDocuments, string fileName = null)
        {
            var dialog = new SaveFileDialog
            {
                Filter = filter,
                InitialDirectory = Environment.GetFolderPath(initialDirectory)
            };

            if (fileName != null) dialog.FileName = fileName;
            return dialog.ShowDialog() == false ? null : new FileInfo(dialog.FileName);
        }

        public static FileInfo SaveCsv()
        {
            return SaveFile("CSV | *.csv");
        }

        public static FileInfo SaveExcel()
        {
            return SaveFile("Excel File | *.xlsx");
        }

        public static FileInfo SavePdf(string fileName = null)
        {
            return SaveFile("PDF | *.pdf", fileName: fileName);
        }

        public static FileInfo BrowseForCsv()
        {
            return BrowseForFile("CSV | *.csv");
        }

    }
}
