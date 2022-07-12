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
        private string churchAddress = Properties.Settings.Default.churchAdress;
        private string churchSlogan = Properties.Settings.Default.churchSlogan;
        User user;
        Parish parish;

        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            parish = new(churchName, churchAddress, churchSlogan);
            DataContext = parish;
            lblUser.Content = "Welcome " + user.Username;
            lblUserType.Content = "User Type: " + user.Priviledge;
            lblLoginTime.Content = "Login Time: " + DateTime.Now.ToString();
            frame.LoadCompleted += frame_LoadCompleted;
            
        }

        private  void frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            while (frame.CanGoBack)
            {
                _ = frame.RemoveBackEntry();
            }
        }

        private void btnAddNewOrganisation_Click(object sender, RoutedEventArgs e)
        {
            openPage(new pgAddNewOrganisation(parish));
        }

        private void btnAddNewParishioner_Click(object sender, RoutedEventArgs e)
        {
            openPage(new pgAddNewParishioner(user, parish));
        }

        private void btnAddNewUser_Click(object sender, RoutedEventArgs e)
        {
            openPage(new pgAddNewUser());
        }

        private void openPage(Page page)
        {
            if (frame.Content == null)
            {
                _ = frame.Navigate(page);
            }
            else
            {
                if (frame.Content.ToString() != page.ToString())
                {
                    _ = frame.Navigate(page);
                }
            }
        }

        private void btnAddNewStation_Click(object sender, RoutedEventArgs e)
        {
            openPage(new pgAddNewStation(parish));
        }

        private void btnAddNewWorkingSociety_Click(object sender, RoutedEventArgs e)
        {
            openPage(new pgAddNewWorkingSociety(parish));
        }

        private void btnAddNewApostolate_Click(object sender, RoutedEventArgs e)
        {
            openPage(new pgAddLayApostolate(parish));
        }

        private void btnAddNewOtherSociety_Click(object sender, RoutedEventArgs e)
        {
            openPage(new pgAddOtherSociety(parish));
        }

        private void btnAddNewPiousSociety_Click(object sender, RoutedEventArgs e)
        {
            openPage(new pgAddPiousSociety(parish));
        }

        private void btnAddNewPriest_Click(object sender, RoutedEventArgs e)
        {
            openPage(new pgAddPriest(parish));
        }
    }
}
