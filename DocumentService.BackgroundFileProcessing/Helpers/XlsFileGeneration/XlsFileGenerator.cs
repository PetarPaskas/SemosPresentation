using DocumentService.BackgroundFileProcessing.Domain;
using DocumentService.BackgroundFileProcessing.Processes;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentService.BackgroundFileProcessing.Helpers.XlsFileGeneration
{
    public class XlsFileGenerator : IFileGenerator<XlsFileMetadata>
    {
        public byte[] Process(XlsFileMetadata input)
        {
            var rowToWrite = input.Row;
            var data = input.Items.ToArray();
            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("SheetOne");

                for (int i = 0; i < data.Count(); i++)
                {
                    worksheet.SetValue(rowToWrite, i+1, data[i]);
                }

                using var memStream = new MemoryStream();
                package.SaveAs(memStream);
                return memStream.ToArray();
            }
        }
    }
}
