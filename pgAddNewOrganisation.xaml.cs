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
    /// Interaction logic for pgAddNewOrganisation.xaml
    /// </summary>
    public partial class pgAddNewOrganisation : Page
    {
        Utility utility = new();
        Parish parish;

        public pgAddNewOrganisation(Parish parish)
        {
            InitializeComponent();
            this.parish = parish;
        }

        private void btnCreateOrg_Click(object sender, RoutedEventArgs e)
        {
            lblOrgAcronymError.Content = "";
            lblOrgNameError.Content = "";
            lblOrgSloganError.Content = "";

            bool hasError = false;
            string organisationName = txtOrgName.Text;
            string organisationSlogan = txtOrgSlogan.Text;
            string organisationAcronymn = txtOrgAcronym.Text;

            if (utility.checkNullOrEmpty(organisationName)) { lblOrgNameError.Content = "Organisation name is required"; hasError = true; }
            if (utility.checkNullOrEmpty(organisationSlogan)) { lblOrgSloganError.Content = "Organisation slogan is required"; hasError = true; }
            if (utility.checkNullOrEmpty(organisationAcronymn)) { lblOrgAcronymError.Content = "Organisation acronym is required"; hasError = true; }

            if (!hasError)
            {
                bool result = parish.addOrganisation(organisationName, organisationAcronymn, organisationSlogan);
                if (result)
                {
                    MessageBox.Show("Organisation successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error occurred while adding organisation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
