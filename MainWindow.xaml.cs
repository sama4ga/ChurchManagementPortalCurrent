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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChurchManagementPortal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQL sql = new();
        private string churchName = Properties.Settings.Default.churchName;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAddNewOrganisation_Click(object sender, RoutedEventArgs e)
        {
            winLogin login = new();
            login.Show();
        }

        private void btnAddNewParishioner_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new pgAddNewParishioner();
        }

        private void btnAddNewUser_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new pgAddNewUser();
        }
    }
}
