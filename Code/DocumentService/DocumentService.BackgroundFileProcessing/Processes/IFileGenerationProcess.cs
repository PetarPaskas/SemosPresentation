﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentService.BackgroundFileProcessing.Processes
{
    public interface IFileGenerationProcess
    {
        Task Process();
    }
}
