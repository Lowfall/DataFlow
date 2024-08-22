using DataFlow.Data;
using DataFlow.Interfaces;
using DataFlow.Model;
using DataFlow.Services;
using DataFlow.Utilities;
using DataFlow.View.ModalPages;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace DataFlow.ViewModel
{
    public class FilesViewModel : ViewModelBase
    {
        Stack<string> navigationHistory = new Stack<string>();
        readonly string PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string buttonVisibility = "Visible";
        string listVisibility = "Collapsed";
        string currentDirectory;
        int loadedRows;
        int rowsAmount;
        IFilesService filesService;

        public RelayCommand ChangeFolderCommand { get; set; }
        public RelayCommand ReturnBackCommand { get; set; }
        public RelayCommand ChooseFolderCommand { get; set; }
        public RelayCommand OpenGenerateModalCommand { get; set; }
        public RelayCommand OpenMergeModalCommand { get; set; }
        public RelayCommand ImportFileCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }

        public string CurrentDirectory
        {
            get { return currentDirectory; }
            set { currentDirectory = value; OnPropertyChanged("CurrentDirectory"); navigationHistory.Push(currentDirectory); LoadContent(); }
        }
        public string ButtonVisibility
        {
            get { return buttonVisibility; }
            set { buttonVisibility = value; OnPropertyChanged("ButtonVisibility"); }
        }
        public string ListVisibility
        {
            get { return listVisibility; }
            set { listVisibility = value; OnPropertyChanged("ListVisibility"); }
        }
        public int RowsAmount
        {
            get { return rowsAmount; }
            set { rowsAmount = value; OnPropertyChanged("RowsAmount"); }
        }
        public int LoadedRows
        {
            get { return loadedRows; }
            set { loadedRows = value; OnPropertyChanged("LoadedRows"); }
        }
        public ObservableCollection<FileManagerItem> FolderContent{ get; set; }

        public FilesViewModel()
        {
            ChangeFolderCommand = new RelayCommand(param => ChangeFolder((FileManagerItem)param)) ;
            ReturnBackCommand = new RelayCommand(param => ReturnBack());
            ChooseFolderCommand = new RelayCommand(param => ChooseFolder());
            OpenGenerateModalCommand = new RelayCommand(param => OpenGenerateModal());
            OpenMergeModalCommand = new RelayCommand(param => OpenMergeModal());
            ImportFileCommand = new RelayCommand(async param =>await  ImportFileAsync());
            RefreshCommand = new RelayCommand(param => LoadContent());
            FolderContent = new ObservableCollection<FileManagerItem>();
            filesService = new FilesService(CurrentDirectory);
            LoadContent();
        }

        private void LoadContent()
        {
            FolderContent.Clear();
            try
            {
                if (Directory.Exists(CurrentDirectory))
                {
                    string fileIcon = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Resources", "File.png");
                    string folderIcon = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Resources", "Folder.png");

                    foreach (var dir in Directory.GetDirectories(CurrentDirectory))
                    {
                        FolderContent.Add(new FileManagerItem { Name = Path.GetFileName(dir), Path = dir, IsDirectory = true, Icon = folderIcon });
                    }

                    foreach (var file in Directory.GetFiles(CurrentDirectory))
                    {
                        FolderContent.Add(new FileManagerItem { Name = Path.GetFileName(file), Path = file, IsDirectory = false, Icon = fileIcon });
                    }

                    ButtonVisibility = "Collapsed";
                    ListVisibility = "Visible";
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ReturnBack();
            }
            catch(Exception ex) {
                MessageBox.Show("Unexpected exception", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ChangeFolder(FileManagerItem item)
        {
            if (item.IsDirectory)
            { 
                CurrentDirectory = item.Path;
            }
        }

        private void ReturnBack()
        {
            if (navigationHistory.Count > 1)
            {
                navigationHistory.Pop();
                CurrentDirectory = navigationHistory.Pop(); 
            }
        }

        private void ChooseFolder()
        {
            var folderDialog = new OpenFolderDialog();

            if (folderDialog.ShowDialog() == true)
            {
                navigationHistory.Clear();
                var folderName = folderDialog.FolderName;
                CurrentDirectory = folderName;
            }
        }

        private void OpenGenerateModal()
        {
            new GenerateModalPage(CurrentDirectory).ShowDialog();
        }
        private void OpenMergeModal()
        {
            new MergeModalPage(CurrentDirectory).ShowDialog();
        }

        private async Task ImportFileAsync()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var folderDialog = new OpenFileDialog();

            if (folderDialog.ShowDialog() == true)
            {
                var lines = await File.ReadAllLinesAsync(folderDialog.FileName);
                RowsAmount = lines.Length;
                foreach (var line in lines)
                {
                    var words = line.Split("||").ToArray();
                    var model = MapToModel(words);
                    await dbContext.FilesData.AddAsync(model);
                    await dbContext.SaveChangesAsync();
                    LoadedRows++;
                }
            }
        }

        private FileData MapToModel(string[] lines)
        {
            var fileData = new FileData()
            {
                Date = DateTime.Parse(lines[0]),
                EnglishString = lines[1],
                RussianString = lines[2],
                GeneratedInteger = Int32.Parse(lines[3]),
                GeneratedFloat = Single.Parse(lines[4])
            };
            return fileData;
        }
    }
}
