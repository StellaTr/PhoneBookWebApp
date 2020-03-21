using PhoneBookWebApp.Entities;
using PhoneBookWebApp.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhoneBookWebApp.Test
{
    public class PhoneBookRepositoryUnitTests
    {
        [Fact]
        public async Task GetContactById_ContactExists()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(GetContactById_ContactExists));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            Contact contact = await repository.GetContact(1);

            dbContext.Dispose();

            Assert.NotNull(contact);
            Assert.NotEmpty(contact.ContactPhones);
        }

        [Fact]
        public async Task GetContactById_ContactDoesNotExist()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(GetContactById_ContactDoesNotExist));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            Contact contact = await repository.GetContact(1000);

            dbContext.Dispose();

            Assert.Null(contact);
        }

        [Fact]
        public async Task SearchContactsByFistName_ContactExists()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(SearchContactsByFistName_ContactExists));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            string firstName = "John";
            IEnumerable<Contact> contacts = await repository.SearchContactsByNameAsync(firstName);

            dbContext.Dispose();

            Assert.NotNull(contacts);
            Assert.All<Contact>(contacts, c => c.FirstName.Equals(firstName));
        }

        [Fact]
        public async Task SearchContactsByLastName_ContactExists()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(SearchContactsByLastName_ContactExists));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            string lastName = "Doe";
            IEnumerable<Contact> contacts = await repository.SearchContactsByNameAsync(lastName);

            dbContext.Dispose();

            Assert.NotNull(contacts);
            Assert.All<Contact>(contacts, c => c.LastName.Equals(lastName));
        }

        [Fact]
        public async Task SearchContactsByLastName_ContactDoesNotExist()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(SearchContactsByLastName_ContactDoesNotExist));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            string lastName = "Brown";
            IEnumerable<Contact> contacts = await repository.SearchContactsByNameAsync(lastName);

            dbContext.Dispose();

            Assert.Equal(new List<Contact>(), contacts);
        }

        [Fact]
        public async Task SearchContactsByPhone_ContactExists()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(SearchContactsByPhone_ContactExists));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            string phone = "322209999999";
            IEnumerable<Contact> contacts = await repository.SearchContactsByPhoneAsync(phone);

            dbContext.Dispose();

            Assert.NotNull(contacts);
            Assert.Single(contacts);
            Assert.Single(contacts.First().ContactPhones);
            Assert.All<Contact>(contacts,
                c => string.Concat(c.ContactPhones.First().CountryCode,
                c.ContactPhones.First().AreaCode,
                c.ContactPhones.First().PhoneNumber)
                .Equals(phone));
        }

        [Fact]
        public async Task SearchContactsByPhone_ContactDoesNotExist()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(SearchContactsByPhone_ContactDoesNotExist));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            string phone = "7845129317";
            IEnumerable<Contact> contacts = await repository.SearchContactsByPhoneAsync(phone);

            dbContext.Dispose();

            Assert.Equal(new List<Contact>(), contacts);
        }

        [Fact]
        public async Task ContactExists_BooleanResponse_ContactExists()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(ContactExists_BooleanResponse_ContactExists));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            int id = 2;
            bool contactExists = await repository.ContactExistsAsync(id);

            dbContext.Dispose();

            Assert.True(contactExists);
        }

        [Fact]
        public async Task ContactExists_BooleanResponse_ContactDoesNotExist()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(ContactExists_BooleanResponse_ContactDoesNotExist));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            int id = 2000;
            bool contactExists = await repository.ContactExistsAsync(id);

            dbContext.Dispose();

            Assert.False(contactExists);
        }

        [Fact]
        public async Task FindContactInContext_ContactExists()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(FindContactInContext_ContactExists));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            int id = 2;
            Contact contact = await repository.FindAsync(id);

            dbContext.Dispose();

            Assert.NotNull(contact);
            Assert.Equal(id, contact.ContactId);
            Assert.NotNull(contact.ContactPhones);
            Assert.NotEmpty(contact.ContactPhones);
        }

        [Fact]
        public async Task FindContactInContext_ContactDoesNotExist()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(FindContactInContext_ContactDoesNotExist));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            int id = 2000;
            Contact contact = await repository.FindAsync(id);

            dbContext.Dispose();

            Assert.Null(contact);
        }

        [Fact]
        public async Task GetContactPhone_ContactExists()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(GetContactPhone_ContactExists));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            int contactId = 2;
            int contactPhoneId = 2;

            ContactPhone contactPhone = await repository.GetContactPhone(contactId, contactPhoneId);

            dbContext.Dispose();

            Assert.NotNull(contactPhone);
            Assert.Equal(contactPhoneId, contactPhone.ContactPhoneId);
            Assert.Equal(contactId, contactPhone.ContactId);
            Assert.NotNull(contactPhone.Contact);
        }


        [Fact]
        public async Task AddContact_NewContact()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(AddContact_NewContact));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            Contact newContact = new Contact()
            {
                ContactId = 0,
                FirstName = "Freddie",
                LastName = "Philips",
                ContactPhones = new List<ContactPhone>()
                {
                    new ContactPhone()
                    {
                        ContactId = 0,
                        ContactPhoneId = 0,
                        CountryCode = "40",
                        AreaCode = "80",
                        PhoneNumber = "74521397"
                    }
                }
            };

            repository.AddContact(newContact);
            await repository.SaveAsync();

            Assert.True(dbContext.Contacts.Any(c => c.FirstName.Equals("Freddie") && c.LastName.Equals("Philips")));

            Assert.NotEmpty(dbContext.Contacts.First(c => c.FirstName.Equals("Freddie") && c.LastName.Equals("Philips")).ContactPhones);

            dbContext.Dispose();
        }

        [Fact]
        public async Task AddContact_NewContactPhone()
        {
            var dbContext = DbContextMocker.GetPhoneBookDbContext(nameof(AddContact_NewContactPhone));
            PhoneBookRepository repository = new PhoneBookRepository(dbContext);

            Contact newContact = new Contact()
            {
                ContactId = 0,
                FirstName = "John",
                LastName = "Doe",
                ContactPhones = new List<ContactPhone>()
                {
                    new ContactPhone()
                    {
                        ContactPhoneId = 0,
                        ContactId = 0,
                        CountryCode = "00",
                        AreaCode = "001",
                        PhoneNumber = "123456"
                    }
                }
            };

            repository.AddContact(newContact);
            await repository.SaveAsync();

            Assert.True(dbContext.Contacts.Where(c => c.FirstName.Equals("John") && c.LastName.Equals("Doe")).Count() == 1);

            Assert.NotEmpty(dbContext.Contacts.First(c => c.FirstName.Equals("John") && c.LastName.Equals("Doe")).ContactPhones);

            Assert.Contains(dbContext.Contacts
                .First(c => c.FirstName.Equals("John") && c.LastName.Equals("Doe"))
                .ContactPhones, p => (p.CountryCode + p.AreaCode + p.PhoneNumber).Equals("00001123456"));

            dbContext.Dispose();
        }
    }
}
