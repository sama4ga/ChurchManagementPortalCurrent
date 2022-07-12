using Microsoft.Win32;
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
using System.IO;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChurchManagementPortal
{
    /// <summary>
    /// Interaction logic for pgAddNewStation.xaml
    /// </summary>
    public partial class pgAddNewStation : Page
    {
        Utility utility = new();
        string filename = "";
        Parish parish;

        public pgAddNewStation(Parish parish)
        {
            InitializeComponent();
            dtpCreationDate.SelectedDate = DateTime.Now;
            this.parish = parish;
        }

        private void btnCreateStation_Click(object sender, RoutedEventArgs e)
        {
            lblStationAddressError.Content = "";
            lblStationNameError.Content = "";
            lblCreationDateError.Content = "";
            lblPictureError.Content = "";

            string stationName = txtStationName.Text.Trim();
            string stationAddress = txtStationAddress.Text.Trim();
            DateTime? creationDate = dtpCreationDate.SelectedDate;
            bool hasError = false;

            if (utility.checkNullOrEmpty(stationName)) { lblStationNameError.Content = "Enter station name to proceed"; hasError = true; }
            if (utility.checkNullOrEmpty(stationAddress)) { lblStationAddressError.Content = "Enter station adress to proceed"; hasError = true; }
            if (creationDate == null) { lblCreationDateError.Content = "Enter station creation date to proceed"; hasError = true; }

            if (!hasError)
            {
                string date = creationDate.Value.ToString("yyyy-MM-d");
                bool result = parish.addStation(stationName, stationAddress, date, filename);
                if (result)
                {
                    MessageBox.Show("Station successfully created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("There was an error while creating station", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnPicture_Click(object sender, RoutedEventArgs e)
        {
            filename = utility.uploadFile("Choose Station Picture", "Image Files|*.jpeg;*.jpg;*.png");
            if (filename != "")
            {
                BitmapImage img = new(new Uri(filename));
                picPicture.Source = img;
            }
        }
    }
}
