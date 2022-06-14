using System;

namespace ChurchManagementPortal
{
    class Death
    {
        public DateTime BurialDate { get; set; }
        public Parishioner Parishioner { get; set; }

        /// <summary>
        /// Initializes the class
        /// </summary>
        /// <param name="parishioner">Parishioner that is dead</param>
        /// <param name="burialDate">Burial data of the parishioner</param>
        public Death(Parishioner parishioner, DateTime burialDate)
        {
            Parishioner = parishioner;
            BurialDate = burialDate;
        }
    }
}
