using System.Windows;

namespace ChurchManagementPortal
{
    public class WorkingSociety : Society
    {
        public Organisation Organisation { get; set; }

        public WorkingSociety(string name, Organisation organisation, Station station) : base(name)
        {
            if (organisation.Station != station)
            {
                MessageBox.Show(string.Format("{0} does not belong to {1}", organisation, station));
                return;
            }
            base.Station = station;
            Organisation = organisation;
        }

        public override string ToString()
        {
            return base.Name + ", " + base.Station;
        }
    }
}