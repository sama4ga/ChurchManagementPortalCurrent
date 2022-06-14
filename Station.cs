namespace ChurchManagementPortal
{
    public class Station
    {
        public string Name { get; set; }

        public Station(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}