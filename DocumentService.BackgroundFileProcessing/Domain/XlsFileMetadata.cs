using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentService.BackgroundFileProcessing.Domain
{
    public class XlsFileMetadata
    {
        public int Row { get; }
        public IEnumerable<string> Items { get; }
        public XlsFileMetadata(int row, IEnumerable<string> items)
        {
            Row = row;
            Items = items;
        }
    }
}
