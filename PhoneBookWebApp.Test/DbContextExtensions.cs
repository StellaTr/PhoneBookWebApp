using PhoneBookWebApp.Data;
using PhoneBookWebApp.Entities;
using System.Collections.Generic;


namespace PhoneBookWebApp.Test
{
    public static class DbContextExtensions
    {
        public static void Seed(this PhoneBookContext dbContext)
        {
            dbContext.Contacts.Add(new Contact()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                ContactPhones = new List<ContactPhone>()
                {
                    new ContactPhone()
                    {
                        ContactPhoneId = 1,
                        ContactId = 1,
                        CountryCode = "30",
                        AreaCode = "478",
                        PhoneNumber = "123456"
                    }
                }
            });

            dbContext.Contacts.Add(new Contact()
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                ContactPhones = new List<ContactPhone>()
                {
                    new ContactPhone()
                    {
                        ContactPhoneId = 2,
                        ContactId = 2,
                        CountryCode = "32",
                        AreaCode = "220",
                        PhoneNumber = "9999999"
                    },
                    new ContactPhone()
                    {
                        ContactPhoneId = 3,
                        ContactId = 2,
                        CountryCode = "30",
                        AreaCode = "220",
                        PhoneNumber = "45781293"
                    }
                }
            });

            dbContext.Contacts.Add(new Contact()
            {
                ContactId = 3,
                FirstName = "Larry",
                LastName = "Page",
                ContactPhones = new List<ContactPhone>()
                {
                    new ContactPhone()
                    {
                        ContactPhoneId = 4,
                        ContactId = 3,
                        CountryCode = "44",
                        AreaCode = "880",
                        PhoneNumber = "9623148"
                    },
                    new ContactPhone()
                    {
                        ContactPhoneId = 5,
                        ContactId = 3,
                        CountryCode = "44",
                        AreaCode = "880",
                        PhoneNumber = "78451236"
                    }
                }
            });

            dbContext.SaveChanges();
        }
    }
}
