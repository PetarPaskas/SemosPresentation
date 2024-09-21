using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentService.BackgroundFileProcessing.Domain;

internal interface IFileGenerator<TInput>
{
    byte[] Process(TInput input);
}
