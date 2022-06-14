namespace ChurchManagementPortal
{
    public class PiousSociety : Society
    {

        public PiousSociety(string name) : base(name)
        {

        }

        public override string ToString()
        {
            return base.Name;
        }
    }
}