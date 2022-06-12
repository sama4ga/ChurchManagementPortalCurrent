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
using System.ServiceProcess;
using MySql.Data.MySqlClient;

namespace ChurchManagementPortal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQL sql = new();

        public MainWindow()
        {
            InitializeComponent();
            if (sql.ExecuteQuery("SELECT * FROM `user`;")){ _ = MessageBox.Show("Success"); }
        }
    }
}
