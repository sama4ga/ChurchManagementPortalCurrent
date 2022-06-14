using System.Collections.Generic;
using System.Text;

namespace ChurchManagementPortal
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Person
    {
        public string Surname { get; set; }
        public string Othernames { get; set; }
        protected string name;
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public string DOB { get; set; }
        public bool Baptised { get; set; }
        public bool Confirmed { get; set; }
        public bool Communion { get; set; }
        protected List<string> phoneNumbers;
        public string Email { get; set; }

        protected Person()
        {
            phoneNumbers = new List<string>();
        }

        public void AddPhoneNumber(string phoneNumber)
        {
            phoneNumbers.Add(phoneNumber);
        }

        public string GetPhoneNumbers()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string number in phoneNumbers)
            {
                sb.Append(number);
            }
            return sb.ToString();
        }

        public string Name
        {
            get
            {
                name = Surname + ", " + Othernames;
                return name;
            }
            /*private set
            {
                name = Surname + ", " + Othernames;
            }*/
            set
            {
                name = value;
            }
        }
    }
}