using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBookWebApp.Models
{
    public class ContactDto
    {
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<ContactPhoneDto> ContactPhones { get; set; }
    }
}
