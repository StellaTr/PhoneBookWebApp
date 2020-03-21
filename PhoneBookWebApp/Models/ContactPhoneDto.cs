using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBookWebApp.Models
{
    public class ContactPhoneDto
    {
        public int ContactPhoneId { get; set; }
        public int ContactId { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
