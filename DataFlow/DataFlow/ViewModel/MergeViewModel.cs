using DataFlow.Interfaces;
using DataFlow.Services;
using DataFlow.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataFlow.ViewModel
{
    public class MergeViewModel : ViewModelBase
    {
        string outputFile = "Result_File";
        string filesPrefix = "Generated_File";
        string currentDirectory;
        string substring = String.Empty;
        IFilesService filesService;

        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand DragMoveWindowCommand { get; set; }
        public RelayCommand MergeFilesCommand { get; set; }

        public string FilesPrefix
        {
            get { return filesPrefix; }
            set { filesPrefix = value; OnPropertyChanged("FilesPrefix"); }
        }
        public string OutputFile
        {
            get { return outputFile; }
            set { outputFile = value; OnPropertyChanged("OutputFile"); }
        }
        public string Substring
        {
            get { return substring; }
            set { substring = value; OnPropertyChanged("Substring"); }
        }

        public MergeViewModel(string currentDirectory)
        {
            CloseWindowCommand = new RelayCommand(param => CloseWindow(param));
            DragMoveWindowCommand = new RelayCommand(param => DragMoveWindow(param));
            MergeFilesCommand = new RelayCommand(async param => await MergeFilesAsync());
            this.currentDirectory = currentDirectory;
            filesService = new FilesService(currentDirectory);
        }

        private async Task MergeFilesAsync()
        {
            if (!String.IsNullOrEmpty(currentDirectory) && !String.IsNullOrEmpty(FilesPrefix) && !String.IsNullOrEmpty(OutputFile))
                await filesService.MergeFilesAsync(OutputFile, FilesPrefix, currentDirectory, Substring);
            else if (currentDirectory is null)
                MessageBox.Show("Please  choose folder");
            else
                MessageBox.Show("Please enter correct data");
        }

        private void CloseWindow(object windowParam)
        {
            if (windowParam is Window window)
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
    }
}
