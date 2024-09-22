using DocumentService.BackgroundFileProcessing.Domain;
using DocumentService.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentService.BackgroundFileProcessing.Mappers
{
    public class XlsFileMetadataMapper : IMapper<XlsFileInputDeliveryContentV1, XlsFileMetadata>
    {
        public XlsFileMetadata Map(XlsFileInputDeliveryContentV1 source)
        {
            int row = source.Row ?? 0;

            string[] items = new string[source.Items.Count()];
            int i = 0;

            foreach(var item in source.Items)
            {
                items[i] = item;
                i++;
            }
            
            return new XlsFileMetadata(row, items);
        }
    }
}
