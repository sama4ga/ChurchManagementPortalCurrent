using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for pgAddNewParishioner.xaml
    /// </summary>
    public partial class pgAddNewParishioner : Page
    {
        User user;
        Parish parish;
        Utility utility = new();
        SQL sql = new();
        string passportFile = "";

        public pgAddNewParishioner(User user, Parish parish)
        {
            InitializeComponent();
            this.user = user;
            this.parish = parish;
            sql.fillComboBox("SELECT `organisationId`, `name` FROM `organisation` ORDER BY `name`;", cmbOrganisation);
            sql.fillComboBox("SELECT `dioceseId`, `name` FROM `diocese` ORDER BY `name`;", cmbDiocese);
            sql.fillComboBox("SELECT `stationId`, `name` FROM `station`;", cmbStation);
            sql.fillComboBox("SELECT `stateId`, `name` FROM `state` ORDER BY `name`;", cmbStateOfOrigin);
            sql.fillListBox("SELECT `groupId`, `name` FROM `group` WHERE `groupType`='Lay Apostolate' ORDER BY `name`;", lstLayApostolates);
            sql.fillListBox("SELECT `groupId`, `name` FROM `group` WHERE `groupType`='Other Society' ORDER BY `name`;", lstOtherSocieties);
            sql.fillListBox("SELECT `groupId`, `name` FROM `group` WHERE `groupType`='Pious Society' ORDER BY `name`;", lstPiousSocieties);
            rdSingle.IsChecked = true;
            dtpDOB.SelectedDate = DateTime.Now;
        }

        private void btnLoadPassport_Click(object sender, RoutedEventArgs e)
        {
            passportFile = utility.uploadFile("Choose Passport File", "Image Files|*.jpeg;*.jpg;*.png");
            if (passportFile != "")
            {
                BitmapImage img = new(new Uri(passportFile));
                picPassport.Source = img;
            }
        }

        private void cmbDiocese_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string dioceseId = cmbDiocese.SelectedValue.ToString();
            sql.fillComboBox("SELECT `deaneryId`,`name` FROM `deanery` WHERE `dioceseId`='" + dioceseId + "' ORDER BY `name`;", cmbDeanery);
        }

        private void cmbDeanery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string deaneryId = cmbDeanery.SelectedValue.ToString();
            sql.fillComboBox("SELECT `parishId`,`name` FROM `parish` WHERE `deaneryId`='" + deaneryId + "' ORDER BY `name`;", cmbParish);
        }

        private void cmbOrganisation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string organisationId = cmbOrganisation.SelectedValue.ToString();
            string stationId = cmbStation.SelectedValue.ToString();
            sql.fillComboBox("SELECT `societyId`,`name` FROM `society` WHERE `organisationId`='" + organisationId + "' AND `stationId`='" + stationId + "' ORDER BY `name`;", cmbSociety);
        }

        private void cmbStateOfOrigin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string stateId = cmbStateOfOrigin.SelectedValue.ToString();
            sql.fillComboBox("SELECT `lgaId`,`name` FROM `lga` WHERE `stateId`='" + stateId + "' ORDER BY `name`;", cmbLGA);
        }

        private void btnAddParishioner_Click(object sender, RoutedEventArgs e)
        {
            string gender = "";
            string maritalStatus = "";
            bool hasError = false;

            if (rdMale.IsChecked == true)
            {
                gender = "Male";
            }else if (rdFemale.IsChecked== true)
            {
                gender = "Female";
            }
            else
            {
                MessageBox.Show("Select your gender to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }

            if (rdSingle.IsChecked == true)
            {
                maritalStatus = "Single";
            }
            else if (rdMarried.IsChecked == true)
            {
                maritalStatus = "Married";
            }
            else if (rdWidow.IsChecked == true)
            {
                maritalStatus = "Widow";
            }
            else if (rdWidower.IsChecked == true)
            {
                maritalStatus = "Widower";
            }
            else
            {
                MessageBox.Show("Select your marital status to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }

            string spouseName = maritalStatus == "Single" ? "" : txtSpouseName.Text;
            if (utility.checkNullOrEmpty(spouseName))
            {
                MessageBox.Show("Enter your spouse name to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            string dob = dtpDOB.SelectedDate.Value.ToString("yyyy-MM-dd");
            string surname = txtSurname.Text;
            if (utility.checkNullOrEmpty(surname))
            {
                MessageBox.Show("Enter your surname to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            string othernames = txtOthernames.Text;
            if (utility.checkNullOrEmpty(othernames))
            {
                MessageBox.Show("Enter your othernames to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            string email = txtEmail.Text;
            string resAddress = txtResAddress.Text;
            if (utility.checkNullOrEmpty(resAddress))
            {
                MessageBox.Show("Enter your residential address to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            string officeAddress = txtOfficeAddress.Text;
            string phoneNo = txtPhoneNo.Text;
            if (utility.checkNullOrEmpty(phoneNo))
            {
                MessageBox.Show("Enter your phone number to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            if (cmbStateOfOrigin.SelectedIndex == -1)
            {
                MessageBox.Show("Select your state of origin to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            else
            {
                string stateOfOrigin = cmbStateOfOrigin.SelectedValue.ToString();
            }
            if (cmbLGA.SelectedIndex == -1)
            {
                MessageBox.Show("Select your LGA of origin to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            else
            {
                string lgaOfOrigin = cmbLGA.SelectedValue.ToString();
            }
            string occupation = txtOccupation.Text;
            if (cmbDiocese.SelectedIndex == -1)
            {
                MessageBox.Show("Select your diocese of origin to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            else
            {
                string dioceseId = cmbDiocese.SelectedValue.ToString();
            }
            if (cmbDeanery.SelectedIndex == -1)
            {
                MessageBox.Show("Select your deanery of origin to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            else
            {
                string deaneryId = cmbDeanery.SelectedValue.ToString();
            }
            if (cmbParish.SelectedIndex == -1)
            {
                MessageBox.Show("Select your parish of origin to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            else
            {
                string parishId = cmbParish.SelectedValue.ToString();
            }
            if (cmbStation.SelectedIndex == -1)
            {
                MessageBox.Show("Select a station proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            else
            {
                string stationId = cmbStation.SelectedValue.ToString();
            }
            if (cmbOrganisation.SelectedIndex == -1)
            {
                MessageBox.Show("Select an organisation to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            else
            {
                string organisationId = cmbOrganisation.SelectedValue.ToString();
            }
            if (cmbSociety.SelectedIndex == -1)
            {
                MessageBox.Show("Select a society to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Information);
                hasError = true;
            }
            else
            {
                string societyId = cmbSociety.SelectedValue.ToString();
            }
            string pId = txtPId.Text;
            string baptised = chkBaptism.IsChecked == true ? "True" : "False";
            string communicant = chkCommunion.IsChecked == true ? "True" : "False";
            string confirmed = chkConfirmation.IsChecked == true ? "True" : "False";
            string married = chkMarriage.IsChecked == true ? "True" : "False";
            string nextOfKinName = txtNextOfKinName.Text;
            string nextOfKinAddress = txtNextOfKinAddress.Text;
            string nextOfKinPhoneNo = txtNextOfKinPhoneNo.Text;
            string whatCanYouDo = txtWhatCanYouDo.Text;

            List<int> layApostolates = new();
            foreach (DataRowView layApostolate in lstLayApostolates.SelectedItems)
            {
                layApostolates.Add((int)layApostolate.Row.ItemArray[0]);
            }
            List<int> otherSocieties = new();
            foreach (DataRowView otherSociety in lstOtherSocieties.SelectedItems)
            {
                otherSocieties.Add((int)otherSociety.Row.ItemArray[0]);
            }
            List<int> piousSocieties = new();
            foreach (DataRowView piousSociety in lstPiousSocieties.SelectedItems)
            {
                piousSocieties.Add((int)piousSociety.Row.ItemArray[0]);
            }

            if (!hasError)
            {
                bool result = parish.addParishioner(txtPId.Text,txtTitle.Text,surname,othernames,resAddress,phoneNo,gender,maritalStatus,
                    (int)cmbStateOfOrigin.SelectedValue, (int)cmbLGA.SelectedValue, (int)cmbDiocese.SelectedValue, (int)cmbDeanery.SelectedValue,
                    (int)cmbParish.SelectedValue, (int)cmbStation.SelectedValue, (int)cmbOrganisation.SelectedValue, (int)cmbSociety.SelectedValue,
                    dob, nextOfKinName, nextOfKinAddress, nextOfKinPhoneNo, otherSocieties, piousSocieties, layApostolates,
                    whatCanYouDo, officeAddress, spouseName, email, occupation, passportFile, baptised, communicant, confirmed,
                    married, "Active");
                if (result)
                {
                    MessageBox.Show("Registration successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    reset();
                }
                else
                {
                    MessageBox.Show("An error was encountered while registering parishioner", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            reset();   
        }

        private void reset()
        {
            txtEmail.Clear(); txtNextOfKinAddress.Clear(); txtNextOfKinName.Clear(); txtNextOfKinPhoneNo.Clear(); txtOccupation.Clear();
            txtOfficeAddress.Clear(); txtOthernames.Clear(); txtPhoneNo.Clear(); txtResAddress.Clear(); txtSpouseName.Clear(); txtSurname.Clear();
            txtTitle.Clear(); txtWhatCanYouDo.Clear(); cmbDeanery.SelectedIndex = -1; cmbDiocese.SelectedIndex = -1; cmbLGA.SelectedIndex = -1;
            cmbOrganisation.SelectedIndex = -1; cmbParish.SelectedIndex = -1; cmbSociety.SelectedIndex = -1; cmbStateOfOrigin.SelectedIndex = -1;
            cmbStation.SelectedIndex = -1; rdDivorced.IsChecked = false; rdFemale.IsChecked = false; rdMale.IsChecked = false; rdMarried.IsChecked = false;
            rdSingle.IsChecked = false; rdWidow.IsChecked = false; rdWidower.IsChecked = false; picPassport.Source = null; dtpDOB.SelectedDate = DateTime.Now;
        }

        private void rdSingle_Checked(object sender, RoutedEventArgs e)
        {
            stkSpouseName.Visibility = Visibility.Hidden;
        }

        private void rdSingle_Unchecked(object sender, RoutedEventArgs e)
        {
            stkSpouseName.Visibility = Visibility.Visible;
        }
    }
}
