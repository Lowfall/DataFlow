using DataFlow.Utilities;
using System.Windows.Input;

namespace DataFlow.ViewModel
{
    public class NavigationViewModel : ViewModelBase
    {
        private object currentView;

        public ICommand FilesCommand { get; set; }
        public ICommand TableCommand { get; set; }

        public object CurrentView
        {
            get { return currentView; }
            set { currentView = value; OnPropertyChanged("CurrentView"); }
        }

        public NavigationViewModel()
        {
            FilesCommand = new RelayCommand( e => { CurrentView = new FilesViewModel(); });
            TableCommand = new RelayCommand( e  => { CurrentView = new TableViewModel(); }); 

            CurrentView = new FilesViewModel();
        }
    }
}
