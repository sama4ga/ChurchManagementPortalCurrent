using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ChurchManagementPortal
{
    public class Society
    {
        protected string Name { get; set; }
        protected internal Station Station { get; set; }
        private List<Parishioner> members;

        public Society(string name)
        {
            Name = name;
            members = new List<Parishioner>();
        }

        protected void AddMember(Parishioner member)
        {
            members.Add(member);
        }

        protected string GetMembers()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Parishioner member in members)
            {
                sb.Append(member);
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}