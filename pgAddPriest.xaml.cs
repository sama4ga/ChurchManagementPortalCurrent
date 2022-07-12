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
using System.Drawing.Imaging;

namespace ChurchManagementPortal
{
    /// <summary>
    /// Interaction logic for pgAddPriest.xaml
    /// </summary>
    public partial class pgAddPriest : Page
    {
        Parish parish;
        Utility utility = new();
        string picPassportFile = "";

        public pgAddPriest(Parish parish)
        {
            InitializeComponent();
            this.parish = parish;
            dtpDateResumed.SelectedDate = DateTime.Now;
        }

        private void btnAddPriest_Click(object sender, RoutedEventArgs e)
        {
            lblDateResumedError.Content = ""; lblPriestEmailError.Content = ""; lblPriestNameError.Content = "";
            lblPriestPassportError.Content = ""; lblPriestPhoneNoError.Content = ""; lblPriestTypeError.Content = "";
            bool hasError = false;

            if (utility.checkNullOrEmpty(txtPriestName.Text)){ lblPriestNameError.Content = "Priest Name is required"; hasError = true; }
            if (utility.checkNullOrEmpty(txtPriestPhoneNo.Text)) { lblPriestNameError.Content = "Priest Phone number is required"; hasError = true; }

            if(!hasError)
            {
                bool result = parish.addPriest(txtPriestName.Text.Trim(), txtPriestPhoneNo.Text.Trim(), cmbPriestType.Text, dtpDateResumed.SelectedDate.Value.ToString("yyyy-MM-dd"), 
                    picPassportFile, txtPriestEmail.Text.Trim());
                if (result)
                {
                    MessageBox.Show("Priest successfully added", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show("An error occured while adding priest", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void btnPriestPassport_Click(object sender, RoutedEventArgs e)
        {
            picPassportFile = utility.uploadFile("Choose passport file", "Image File|*.jpeg;*.jpg;*'png");
            if (picPassportFile != "")
            {
                BitmapImage img = new(new Uri(picPassportFile));
                picPriestpassport.Source = img;
            }
        }
    }
}
