using PhoneBookWebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBookWebApp.Repository
{
    public interface IPhoneBookRepository
    {
        Task<Contact> GetContact(int id);
        Task<IEnumerable<Contact>> SearchContactsByNameAsync(string name);
        Task<IEnumerable<Contact>> SearchContactsByPhoneAsync(string phone);
        void AddContact(Contact contact);
        Task SaveAsync();
        Task<Contact> FindAsync(int id);
        void UpdateContact(Contact contact);
        Task<bool> ContactExistsAsync(int contactId);
    }
}
