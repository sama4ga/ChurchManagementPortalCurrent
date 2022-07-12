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
    /// Interaction logic for pgAddNewWorkingSociety.xaml
    /// </summary>
    public partial class pgAddNewWorkingSociety : Page
    {
        SQL sql = new();
        Utility utility = new();
        Parish parish;

        public pgAddNewWorkingSociety(Parish parish)
        {
            InitializeComponent();
            this.parish = parish;
            sql.fillComboBox("SELECT `organisationId`,`acronym` FROM `organisation`;", cmbOrganisation);
            sql.fillComboBox("SELECT `stationId`,`name` FROM `station`;", cmbStation);
        }

        private void btnCreateSociety_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;
            lblMeetingDayError.Content = "";
            lblStationError.Content = "";
            lblOrganisationError.Content = "";
            lblSoceityNameError.Content = "";

            if (utility.checkNullOrEmpty(cmbOrganisation.SelectedValue.ToString())){ lblOrganisationError.Content = "Select organisation where society belongs to proceed"; hasError = true; }
            if (utility.checkNullOrEmpty(cmbStation.SelectedValue.ToString())) { lblStationError.Content = "Select station where society belongs to proceed"; hasError = true; }
            if (utility.checkNullOrEmpty(cmbMeetingDay.SelectedValue.ToString())) { lblMeetingDayError.Content = "Select society meeting day to proceed"; hasError = true; }
            if (utility.checkNullOrEmpty(txtSoceityName.Text)) { lblSoceityNameError.Content = "Enter nameof society to proceed"; hasError = true; }
           
            if (!hasError)
            {
                bool result = parish.addWorkingSociety(txtSoceityName.Text, txtSocietySlogan.Text, cmbStation.SelectedValue.ToString(), cmbOrganisation.SelectedValue.ToString(),
                    cmbMeetingDay.Text, cmbMeetingType.Text, cmbMeetingFreq.Text);
                if (result)
                {
                    MessageBox.Show("Society successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error occurred while adding society", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
