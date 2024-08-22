using DataFlow.Interfaces;
using DataFlow.Services;
using DataFlow.Utilities;
using System.Windows;

namespace DataFlow.ViewModel
{
    public class GenerateViewModel : ViewModelBase
    {
        string filesPrefix = "Generated_File";
        string filesAmount = "100";
        string rowsAmount = "100000";
        string currentDirectory;
        IFilesService filesService;
        public string FilesPrefix
        {
            get { return filesPrefix; }
            set { filesPrefix = value; OnPropertyChanged("FilesPrefix"); }
        }
        public string FilesAmount
        {
            get { return filesAmount; }
            set { filesAmount = value; OnPropertyChanged("FilesAmount"); }
        }
        public string RowsAmount
        {
            get { return rowsAmount; }
            set { rowsAmount = value; OnPropertyChanged("RowsAmount"); }
        }

        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand DragMoveWindowCommand { get; set; }
        public RelayCommand GenerateStringCommand { get; set; }

        public GenerateViewModel(string currentDirectory)
        {
            CloseWindowCommand = new RelayCommand(param => CloseWindow(param));
            DragMoveWindowCommand = new RelayCommand(param => DragMoveWindow(param));
            GenerateStringCommand = new RelayCommand(async param => await GenerateFiles());
            this.currentDirectory = currentDirectory;
            filesService = new FilesService(currentDirectory);
        }

        private void CloseWindow(object windowParam)
        {
            if(windowParam is Window window)
            {
                window.Close();
            }
        }

        private void DragMoveWindow(object windowParam)
        {
            if (windowParam is Window window)
            {
                window.DragMove();
            }
        }

        private async Task GenerateFiles()
        {
            if (!String.IsNullOrEmpty(currentDirectory) && !String.IsNullOrEmpty(FilesPrefix) && int.TryParse(FilesAmount,out var filesAmount) && int.TryParse(RowsAmount, out var rowsAmount))
                await filesService.GenerateFilesAsync(currentDirectory, FilesPrefix, filesAmount, rowsAmount);
            else if (currentDirectory is null)
                MessageBox.Show("Please  choose folder");
            else
                MessageBox.Show("Please enter correct data");
        }
    }
}
