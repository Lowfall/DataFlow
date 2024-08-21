using DataFlow.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataFlow.View.ModalPages
{
    /// <summary>
    /// Interaction logic for GenerateModalPage.xaml
    /// </summary>
    public partial class GenerateModalPage : Window
    {
        public GenerateModalPage(string currentDirectory)
        {
            DataContext = new GenerateViewModel(currentDirectory);
            InitializeComponent();
        }
    }
}
