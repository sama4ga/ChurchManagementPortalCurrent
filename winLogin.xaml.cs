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
        Utility utility = new();

        public winLogin()
        {
            InitializeComponent();
            txtUsername.Focus();
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
            if (!hasError)
            {
                if (sql.ReadData("SELECT * FROM `user` WHERE `username`='"+ username +"' AND `password`=sha1('"+ password +"');","login"))
                {
                    DataSet ds = sql.ds;
                    if (ds.Tables["login"].Rows.Count == 1)
                    {
                        int userId = ds.Tables["login"].Rows[0].Field<int>("userId");
                        string priviledge = ds.Tables["login"].Rows[0].Field<string>("priviledge");
                        User user = new(userId, username, password, priviledge);
                        MainWindow main = new(user);
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
