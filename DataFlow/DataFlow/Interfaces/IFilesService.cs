namespace DataFlow.Interfaces
{
    public interface IFilesService
    {
        Task GenerateFilesAsync(string directoryPath, string filesPrefix, int filesAmount, int rowsAmount);
        Task MergeFilesAsync(string outputFile, string filePrefix, string directoryPath, string substring);
        void ImportFile();
    }
}
