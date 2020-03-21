using Microsoft.EntityFrameworkCore;
using PhoneBookWebApp.Data;
using PhoneBookWebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBookWebApp.Repository
{
    public class PhoneBookRepository : IPhoneBookRepository
    {
        private readonly PhoneBookContext _context;

        public PhoneBookRepository(PhoneBookContext context)
        {
            _context = context;
        }

        public async Task<Contact> GetContact(int id)
        {
            var contact = await _context.Contacts
                .Include(c => c.ContactPhones)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ContactId.Equals(id));

            return contact;
        }

        public async Task<IEnumerable<Contact>> SearchContactsByNameAsync(string name)
        {
            IQueryable<Contact> contacts = _context.Contacts
                .Where(c => c.FirstName.Equals(name) || c.LastName.Equals(name))
                .Include(c => c.ContactPhones)
                .AsNoTracking();

            return await contacts.ToListAsync();
        }

        public async Task<IEnumerable<Contact>> SearchContactsByPhoneAsync(string phone)
        {
            IQueryable<Contact> contacts = _context.Contacts.Join(_context.ContactPhones,
                        c => c.ContactId,
                        p => p.ContactId,
                        (c, p) => new { c, p })
                        .Where(c => (c.p.CountryCode + c.p.AreaCode + c.p.PhoneNumber).Equals(phone))
                        .Select(c => new Contact
                        {
                            ContactId = c.c.ContactId,
                            FirstName = c.c.FirstName,
                            LastName = c.c.LastName,
                            ContactPhones = new List<ContactPhone> { c.p }
                        });

            return await contacts.ToListAsync();
        }
    }
}
