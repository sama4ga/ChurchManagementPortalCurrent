using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchManagementPortal
{
    class Confirmation
    {
        public Parishioner Parishioner { get; set; }
        public DateTime DateRecieved { get; set; }

        public Confirmation(Parishioner parishioner, DateTime dateRecieved)
        {
            DateRecieved = dateRecieved;
            Parishioner = parishioner;
        }
    }
}
