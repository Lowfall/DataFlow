using CommunityToolkit.Mvvm.Input;
using DataFlow.Data;
using DataFlow.Model;
using DataFlow.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using WinApplication = Microsoft.Office.Interop.Excel.Application;

namespace DataFlow.ViewModel
{
    public class TableViewModel : ViewModelBase
    {
        bool buttonVisibility = true;
        ExcelService excelService;
        FinancialClass selectedFinancialClass;

        public IRelayCommand ImportCommand { get; set; }

        public bool ButtonVisibility { 
            get { return buttonVisibility; } 
            set {  buttonVisibility = value; OnPropertyChanged("ButtonVisibility"); }
        }
        public FinancialClass SelectedFinancialClass
        {
            get { return selectedFinancialClass; }
            set{  selectedFinancialClass = value; OnPropertyChanged("SelectedFinancialClass"); FilterAccounts(); }
        }
        public ObservableCollection<LoadedFile> LoadedFiles { get; set; } 
        public ObservableCollection<FinancialClass> FinancialClasses  { get; set; } 
        public ObservableCollection<BankAccount> BankAccounts  { get; set; } 

        public TableViewModel()
        {
            LoadedFiles = new ObservableCollection<LoadedFile>();
            FinancialClasses = new ObservableCollection<FinancialClass>();
            BankAccounts = new ObservableCollection<BankAccount>();
            LoadFiles();
            LoadClasses();
            LoadAccounts();

            ImportCommand = new RelayCommand(ImportFile);
        }

        public void LoadFiles()
        {
           ApplicationDbContext dbContext = new ApplicationDbContext();
           LoadedFiles.Clear();
           foreach(var file in dbContext.LoadedFiles.ToList())
            {
                LoadedFiles.Add(file);
            }
        }

        public void LoadClasses()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            FinancialClasses.Clear();
            foreach (var finClass in dbContext.FinancialClasses.ToList())
            {
                FinancialClasses.Add(finClass);
            }
        }

        public void LoadAccounts()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            BankAccounts.Clear();
            foreach (var account in dbContext.BankAccounts.ToList())
            {
                BankAccounts.Add(account);
            }
        }

        public void FilterAccounts()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            BankAccounts.Clear();
            var filteredAccounts = dbContext.BankAccounts.Where(x => x.FinancialClassId == selectedFinancialClass.Id);
            foreach(var account in filteredAccounts)
            {
                BankAccounts.Add(account);
            }
        }

        public void ImportFile()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var folderDialog = new OpenFileDialog();
            if (folderDialog.ShowDialog() == true)
            {
                if(Path.GetExtension(folderDialog.FileName) != ".xlsx")
                {
                    MessageBox.Show("Not Correct format");
                    return;
                }
                FileInfo fileInfo = new FileInfo(folderDialog.FileName);

                if (fileInfo.Exists)
                {
                    
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using var package = new ExcelPackage(fileInfo);
                    if(package.Workbook.Worksheets.Count == 0)
                    {
                        MessageBox.Show("File doesn't have any worksheet");
                        return;
                    }
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;
                    excelService = new ExcelService(worksheet, new LoadedFile { Name = folderDialog.FileName });
                    var classes = excelService.ParseFile(worksheet.Rows.Count());
                    dbContext.FinancialClasses.AddRange(classes);
                    dbContext.SaveChanges();
                   
                }
            }

        }
    }
}
