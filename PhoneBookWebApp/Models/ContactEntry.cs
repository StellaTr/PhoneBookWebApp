using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBookWebApp.Models
{
    public class ContactEntry
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<ContactPhoneEntry> ContactPhones { get; set; }
    }
}
