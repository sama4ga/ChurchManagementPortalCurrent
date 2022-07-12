using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ChurchManagementPortal
{
    /// <summary>
    /// Interaction logic for pgAddNewUser.xaml
    /// </summary>
    public partial class pgAddNewUser : Page
    {
        Utility utility = new();
        SQL sql = new();

        public pgAddNewUser()
        {
            InitializeComponent();
        }

        private void btnAddNewParishioner_Click(object sender, RoutedEventArgs e)
        {
            lblUsernameError.Content = "";
            lblPasswordError.Content = "";
            lblCPasswordError.Content = "";
            lblPriviledgeError.Content = "";

            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string cPassword = txtCPassword.Password;
            string priviledge = cmbPriviledge.Text;
            bool hasError = false;

            if (utility.checkNullOrEmpty(username))
            {
                lblUsernameError.Content = "Username is required";
                hasError = true;
            }
            if (utility.checkNullOrEmpty(password))
            {
                lblPasswordError.Content = "Password is required";
                hasError = true;
            }
            if (utility.checkNullOrEmpty(cPassword))
            {
                lblCPasswordError.Content = "Confirm Password is required";
                hasError = true;
            }
            if (utility.checkNullOrEmpty(priviledge))
            {
                lblPriviledgeError.Content = "Priviledge is required";
                hasError = true;
            }
            if (!utility.checkEquality(password, cPassword))
            {
                _ = MessageBox.Show("The passwords you entered do not match: "+password+" "+cPassword, "Password Mismatch", MessageBoxButton.OK, MessageBoxImage.Error);
                hasError = true;
            }

            if (!hasError)
            {
                if (sql.ExecuteQuery("INSERT INTO `user`(`username`,`password`,`priviledge`) VALUES('"+username+ "',sha1('" + password + "'),'" + priviledge + "');"))
                {
                    _ = MessageBox.Show("User successfully created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearFields();
                }
                else
                {
                    if (sql.error.ErrorCode == 1062)
                    {
                        _ = MessageBox.Show("Username already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        txtUsername.SelectAll(); 
                        txtUsername.Focus();
                    }
                    else
                    {
                        _ = MessageBox.Show("An error occured while creating user", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }

        private void clearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtCPassword.Clear();
            cmbPriviledge.SelectedIndex = -1;
            lblUsernameError.Content = "";
            lblPasswordError.Content = "";
            lblCPasswordError.Content = "";
            lblPriviledgeError.Content = "";
        }
    }
}
