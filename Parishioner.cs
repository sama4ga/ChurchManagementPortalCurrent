using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ChurchManagementPortal
{
    public class Parishioner : Person
    {
        public bool Married { get; set; }
        public Station Station { get; set; }
        public Organisation Organisation { get; set; }
        public WorkingSociety Society { get; set; }
        private List<PiousSociety> piousSocieties;
        private List<OtherSociety> otherSocieties;

        public Parishioner(string name, Station station, Organisation organisation)
        {
            base.name = name;
            this.Organisation = organisation;
            this.Station = station;
            piousSocieties = new List<PiousSociety>();
            otherSocieties = new List<OtherSociety>();
        }

        public void AddPiousSociety(PiousSociety society)
        {
            piousSocieties.Add(society);
        }

        public string GetPiousSocieties()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PiousSociety society in piousSocieties)
            {
                sb.Append(society.ToString());
            }
            return sb.ToString();
        }

        public void AddOtherSociety(OtherSociety society)
        {
            otherSocieties.Add(society);
        }

        public string GetOtherSocieties()
        {
            StringBuilder sb = new StringBuilder();
            foreach (OtherSociety society in otherSocieties)
            {
                sb.Append(society.ToString());
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==========================================");
            sb.AppendLine("Name: " + this.name);
            sb.AppendLine("Gender: " + this.Gender);
            sb.AppendLine("Address: " + this.Address);
            sb.AppendLine("Email: " + this.Email);
            sb.AppendLine("Date of Birth: " + this.DOB);
            sb.AppendLine("Station: " + this.Station);
            sb.AppendLine("Organisation: " + this.Organisation);
            sb.AppendLine("Society: " + this.Society);
            sb.AppendLine("------------Pious Societies---------------");
            foreach (PiousSociety society in piousSocieties)
            {
                sb.AppendLine(society.ToString());
            }
            sb.AppendLine("------------------------------------------");
            sb.AppendLine("------------Other Societies---------------");
            foreach (OtherSociety society in otherSocieties)
            {
                sb.AppendLine(society.ToString());
            }
            sb.AppendLine("------------------------------------------");
            sb.AppendLine("------------Phone Numbers-----------------");
            foreach (string number in phoneNumbers)
            {
                sb.AppendLine(number);
            }
            sb.AppendLine("------------------------------------------");
            sb.AppendLine("Baptised: " + this.Baptised);
            sb.AppendLine("Communion: " + this.Communion);
            sb.AppendLine("Confirmed: " + this.Confirmed);
            sb.AppendLine("Married: " + this.Married);
            sb.AppendLine("=========================================");
            return sb.ToString();
        }
    }
}