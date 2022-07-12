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
    /// Interaction logic for pgAddPiousSociety.xaml
    /// </summary>
    public partial class pgAddPiousSociety : Page
    {
        Parish parish;
        Utility utility = new();

        public pgAddPiousSociety(Parish parish)
        {
            InitializeComponent();
            this.parish = parish;
        }

        private void btnCreateSociety_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;
            lblMeetingDayError.Content = "";
            lblSocietyNameError.Content = "";

            if (utility.checkNullOrEmpty(cmbMeetingDay.SelectedValue.ToString())) { lblMeetingDayError.Content = "Select society meeting day to proceed"; hasError = true; }
            if (utility.checkNullOrEmpty(txtSocietyName.Text)) { lblSocietyNameError.Content = "Enter nameof society to proceed"; hasError = true; }

            if (!hasError)
            {
                bool result = parish.addSociety(txtSocietyName.Text, txtSocietySlogan.Text, cmbMeetingDay.Text, cmbMeetingType.Text, cmbMeetingFreq.Text, "Pious Society");
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
