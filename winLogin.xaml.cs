using System.Windows;
using System.Data;

namespace ChurchManagementPortal
{
    /// <summary>
    /// Interaction logic for winLogin.xaml
    /// </summary>
    public partial class winLogin : Window
    {
        SQL sql = new();

        public winLogin()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            lblPasswordError.Content = "";
            lblUsernameError.Content = "";
            string username = txtUsername.Text;
            string password = txtPasssword.Password;
            bool error = false;
            if (string.IsNullOrWhiteSpace(username.Trim()))
            {
                lblUsernameError.Content = "Username is required";
                error = true;
            }
            if (string.IsNullOrWhiteSpace(password.Trim()))
            {
                lblPasswordError.Content = "Password is required";
                error = true;
            }
            if (!error)
            {
                if (sql.ReadData("SELECT * FROM `user` WHERE `username`='"+ username +"' AND `password`=sha1('"+ password +"');","login"))
                {
                    DataSet ds = sql.ds;
                    if (ds.Tables["login"].Rows.Count == 1)
                    {
                        int userId = ds.Tables["login"].Rows[0].Field<int>("userId");
                        MainWindow main = new();
                        main.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username and/or password entered", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username and/or password entered: "+ sql.error.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }
    }
}
