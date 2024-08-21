using DataFlow.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataFlow.Services
{
    public class FilesService : IFilesService
    {
        const string rusAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        const string enAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Random randomizer;

        public FilesService(string folderPath)
        {
            randomizer = new Random();
        }

        public async Task GenerateFiles(string directoryPath, string filesPrefix, int filesAmount, int rowsAmount)
        {
                var tasks = Enumerable.Range(1, filesAmount).Select(i =>
                {
                    return Task.Run(() =>
                    {
                        string filePath = Path.Combine(directoryPath, $"{filesPrefix}_{i}.txt");
                        CreateFile(filePath, rowsAmount);
                    });
                });

                await Task.WhenAll(tasks);
        }

        public void ImportFile()
        {
            throw new NotImplementedException();
        }

        public void MergeFiles()
        {
            throw new NotImplementedException();
        }

        private string GenerateRandomSet(string alphabet, int setLength)
        {
            StringBuilder stringBuilder = new StringBuilder(setLength);

            for(int i = 0; i < setLength; i++)
            {
                stringBuilder.Append(alphabet[randomizer.Next(alphabet.Length)]);
            }

            return stringBuilder.ToString() + "||";
        }

        private string GenerateRandomDateTime()
        {
            DateTime startRange  = DateTime.Now.AddYears(-5);
            DateTime endRange = DateTime.Now;

            var days = (endRange - startRange).Days;
                
            return (startRange.AddDays(randomizer.Next(days))).ToShortDateString() + "||";
        }

        private string GenerateRandomFloat(float min, float max, int decimalAmount)
        {
            var randomValue = min + (max - min) * randomizer.NextSingle();    
            return Math.Round(randomValue,decimalAmount).ToString() + "||";
        }

        private string GenerateRow()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(GenerateRandomDateTime()); 
            stringBuilder.Append(GenerateRandomSet(enAlphabet,10)); 
            stringBuilder.Append(GenerateRandomSet(rusAlphabet,10)); 
            stringBuilder.Append(randomizer.Next(0, 100000000).ToString() + "||");
            stringBuilder.Append(GenerateRandomFloat(1, 20, 8));

            return stringBuilder.ToString();
        }

        private void CreateFile(string filePath, int rowsAmount)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                for(int i = 0; i < rowsAmount; i++)
                {
                    streamWriter.WriteLine(GenerateRow());
                }
            }
        }
    }
}
