using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow.Interfaces
{
    interface IFilesService
    {
        Task GenerateFiles(string directoryPath, string filesPrefix, int filesAmount, int rowsAmount);
        void MergeFiles();
        void ImportFile();
    }
}
