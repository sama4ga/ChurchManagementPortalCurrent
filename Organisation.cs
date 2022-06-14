namespace ChurchManagementPortal
{
    public class Organisation : Society
    {

        public Organisation(string name, Station station) : base(name)
        {
            base.Station = station;
        }

        public override string ToString()
        {
            return base.Name + ", " + base.Station;
        }
    }
}