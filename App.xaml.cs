using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceProcess;

namespace ChurchManagementPortal
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ServiceController controller = new("MYSQL80");
            if (controller.Status != ServiceControllerStatus.Running)
            {
                controller.Start();
            }

        }
    }
}
