using DataFlow.Data;
using DataFlow.Interfaces;
using DataFlow.Model;
using OfficeOpenXml;

namespace DataFlow.Services
{
    public class ExcelService : IExcelService
    {
        ExcelWorksheet worksheet;
        LoadedFile loadedFile;

        public ExcelService(ExcelWorksheet worksheet, LoadedFile loadedFile)
        {
            this.worksheet = worksheet;
            this.loadedFile = loadedFile;
        }
        public BankAccount ParseRow(int row, FinancialClass financialClass)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            BankAccount account = new BankAccount
            {
                Id = int.Parse(worksheet.Cells[row, 1].Text),
                ActiveOpenningBalance = decimal.Parse(worksheet.Cells[row, 2].Text),
                PassiveOpenningBalance = decimal.Parse(worksheet.Cells[row, 3].Text),
                DebitTurnover = decimal.Parse(worksheet.Cells[row, 4].Text),
                CreditTurnover = decimal.Parse(worksheet.Cells[row, 5].Text),
                ActiveClosingBalance = decimal.Parse(worksheet.Cells[row, 6].Text),
                PassiveClosingBalance = decimal.Parse(worksheet.Cells[row, 7].Text),
                FinancialClass = financialClass,
                FinancialClassId = financialClass.Id
            };
            if(dbContext.BankAccounts.FirstOrDefault(x => x.Id == int.Parse(worksheet.Cells[row, 1].Text)) is not null)
            {
                return null;
            }
            return account; 
        }

        public List<FinancialClass> ParseFile( int rowsAmount)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            List<FinancialClass> financialClasses = new List<FinancialClass>();
            for (int i =9; i < rowsAmount; i++)
            {
                if (worksheet.Cells[i, 1].Style.Font.Bold) {
                    if(worksheet.Cells[i, 1].Text.Contains("КЛАСС ") && dbContext.FinancialClasses.FirstOrDefault(x => x.Name == worksheet.Cells[i, 1].Text) is null)
                    {
                        financialClasses.Add(new FinancialClass
                        {
                            Name = worksheet.Cells[i, 1].Text,
                            LoadedFile = loadedFile,
                            LoadedFileId = loadedFile.Id,
                            BankAccounts = new List<BankAccount>()
                        }) ;
                    }
                    continue;
                }
                
                if(financialClasses.Count  == 0)
                {
                    continue;
                }
                var account = ParseRow(i,financialClasses.Last());
                if (account is not null)
                {
                    financialClasses.Last().BankAccounts.Add(account);
                }
            }
            return financialClasses;
        }

        
    }
}
