using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow.Interfaces
{
    interface IFilesService
    {
        Task GenerateFilesAsync(string directoryPath, string filesPrefix, int filesAmount, int rowsAmount);
        Task MergeFilesAsync(string outputFile, string filePrefix, string directoryPath, string substring);
        void ImportFile();
    }
}
