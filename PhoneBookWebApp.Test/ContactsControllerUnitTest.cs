using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PhoneBookWebApp.Controllers;
using PhoneBookWebApp.Entities;
using PhoneBookWebApp.Models;
using PhoneBookWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhoneBookWebApp.Test
{
    public class ContactsControllerUnitTest
    {
        [Fact]
        public async Task GetContactByContactId_ContactExists()
        {
            Contact contactFromRepo = new Contact()
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
            };

            ContactDto returnedContact = new ContactDto()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                ContactPhones = new List<ContactPhoneDto>()
                {
                    new ContactPhoneDto()
                    {
                        ContactPhoneId = 1,
                        ContactId = 1,
                        CountryCode = "30",
                        AreaCode = "478",
                        PhoneNumber = "123456"
                    }
                }
            };

            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.GetContact(It.IsAny<int>()))
                .ReturnsAsync(contactFromRepo);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ContactDto>(It.IsAny<Contact>()))
                .Returns(returnedContact);

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.GetContact(1);

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result.Value);
            Assert.Equal(returnedContact, result.Value);
        }

        [Fact]
        public async Task GetContactByContactId_ContactDoesNotExist()
        {
            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.GetContact(It.IsAny<int>()))
                .ReturnsAsync((Contact)null);

            var mapperMock = new Mock<IMapper>();

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.GetContact(1);

            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetAllContactsSearchByName_FoundMatchingElements()
        {
            IEnumerable<Contact> contactsFromRepo = new List<Contact>()
            {
                new Contact()
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
                },
                new Contact()
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
                            CountryCode = "44",
                            AreaCode = "15",
                            PhoneNumber = "784512"
                        }
                    }
                }
            };

            IEnumerable<ContactDto> returnedContacts = new List<ContactDto>()
            {
               new ContactDto()
               {
                    ContactId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    ContactPhones = new List<ContactPhoneDto>()
                        {
                            new ContactPhoneDto()
                            {
                                ContactPhoneId = 1,
                                ContactId = 1,
                                CountryCode = "30",
                                AreaCode = "478",
                                PhoneNumber = "123456"
                            }
                        }
                },
               new ContactDto()
                {
                    ContactId = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    ContactPhones = new List<ContactPhoneDto>()
                    {
                        new ContactPhoneDto()
                        {
                            ContactPhoneId = 2,
                            ContactId = 2,
                            CountryCode = "44",
                            AreaCode = "15",
                            PhoneNumber = "784512"
                        }
                    }
                }
            };


            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.SearchContactsByNameAsync("Doe"))
                .ReturnsAsync(contactsFromRepo);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<ContactDto>>(It.IsAny<IEnumerable<Contact>>()))
                .Returns(returnedContacts);

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.GetContacts("Doe");

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result.Value);
            Assert.Equal(returnedContacts, result.Value);
        }

        [Fact]
        public async Task GetAllContactsSearchByName_NotFoundMatchingElements()
        {
            IEnumerable<Contact> contactsFromRepo = new List<Contact>()
            {
                new Contact()
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
                },
                new Contact()
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
                            CountryCode = "44",
                            AreaCode = "15",
                            PhoneNumber = "784512"
                        }
                    }
                }
            };

            IEnumerable<ContactDto> returnedContacts = new List<ContactDto>()
            {
               new ContactDto()
               {
                    ContactId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    ContactPhones = new List<ContactPhoneDto>()
                        {
                            new ContactPhoneDto()
                            {
                                ContactPhoneId = 1,
                                ContactId = 1,
                                CountryCode = "30",
                                AreaCode = "478",
                                PhoneNumber = "123456"
                            }
                        }
                },
               new ContactDto()
                {
                    ContactId = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    ContactPhones = new List<ContactPhoneDto>()
                    {
                        new ContactPhoneDto()
                        {
                            ContactPhoneId = 2,
                            ContactId = 2,
                            CountryCode = "44",
                            AreaCode = "15",
                            PhoneNumber = "784512"
                        }
                    }
                }
            };

            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.SearchContactsByNameAsync("Doe"))
                .ReturnsAsync((IEnumerable<Contact>)null);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<ContactDto>>(It.IsAny<IEnumerable<Contact>>()))
                .Returns((IEnumerable<ContactDto>)null);

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.GetContacts("Doe");

            var result = actionResult.Result as OkObjectResult;
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetContactsSearchByPhone_FoundMatchingElements()
        {
            IEnumerable<Contact> contactsFromRepo = new List<Contact>()
            {
                new Contact()
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
                }
            };

            IEnumerable<ContactDto> returnedContacts = new List<ContactDto>()
            {
               new ContactDto()
               {
                    ContactId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    ContactPhones = new List<ContactPhoneDto>()
                        {
                            new ContactPhoneDto()
                            {
                                ContactPhoneId = 1,
                                ContactId = 1,
                                CountryCode = "30",
                                AreaCode = "478",
                                PhoneNumber = "123456"
                            }
                        }
                }
            };

            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.SearchContactsByPhoneAsync("30478123456"))
                .ReturnsAsync(contactsFromRepo);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<ContactDto>>(It.IsAny<IEnumerable<Contact>>()))
                .Returns(returnedContacts);

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.GetContacts("30478123456");

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result.Value);
            Assert.Equal(returnedContacts, result.Value);
        }

        [Fact]
        public async Task GetContactsSearchByPhone_NotFoundMatchingElements()
        {
            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.SearchContactsByNameAsync("30478123756"))
                .ReturnsAsync((IEnumerable<Contact>)null);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<ContactDto>>(It.IsAny<IEnumerable<Contact>>()))
                .Returns((IEnumerable<ContactDto>)null);

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.GetContacts("30478123756");

            var result = actionResult.Result as OkObjectResult;
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task PostContact_SendNullValue()
        {
            var repositoryMock = new Mock<IPhoneBookRepository>();
            var mapperMock = new Mock<IMapper>();

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.PostContact(null);

            Assert.IsAssignableFrom<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostContact_SuccessfulAdd()
        {
            ContactEntry newEnrty = new ContactEntry()
            {
                FirstName = "Mary",
                LastName = "Philips",
                ContactPhones = new List<ContactPhoneEntry>()
                {
                    new ContactPhoneEntry()
                    {
                        CountryCode = "10",
                        AreaCode = "210",
                        PhoneNumber = "78452139"
                    }
                }
            };

            Contact contact = new Contact()
            {
                ContactId = 3,
                FirstName = "Mary",
                LastName = "Philips",
                ContactPhones = new List<ContactPhone>()
                {
                    new ContactPhone()
                    {
                        ContactId = 3,
                        ContactPhoneId = 4,
                        CountryCode = "10",
                        AreaCode = "210",
                        PhoneNumber = "78452139"
                    }
                }
            };

            ContactDto returnedContact = new ContactDto()
            {
                ContactId = 3,
                FirstName = "Mary",
                LastName = "Philips",
                ContactPhones = new List<ContactPhoneDto>()
                {
                    new ContactPhoneDto()
                    {
                        ContactId = 3,
                        ContactPhoneId = 4,
                        CountryCode = "10",
                        AreaCode = "210",
                        PhoneNumber = "78452139"
                    }
                }
            };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Contact>(It.IsAny<ContactEntry>()))
                .Returns(contact);

            mapperMock.Setup(m => m.Map<ContactDto>(It.IsAny<Contact>()))
               .Returns(returnedContact);

            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
               .Setup(m => m.AddContact(contact));

            repositoryMock
               .Setup(m => m.SaveAsync());

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.PostContact(newEnrty);

            var result = actionResult.Result as CreatedAtActionResult;
            Assert.NotNull(result.Value);
            Assert.Equal(returnedContact, result.Value);
        }

        [Fact]
        public async Task UpdateContact_InconsinstentContactId()
        {
            ContactDto contactForUpdate = new ContactDto()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                ContactPhones = new List<ContactPhoneDto>()
                {
                    new ContactPhoneDto()
                    {
                        ContactPhoneId = 1,
                        ContactId = 1,
                        CountryCode = "30",
                        AreaCode = "478",
                        PhoneNumber = "123456"
                    }
                }
            };

            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IPhoneBookRepository>();

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.UpdateContact(2, contactForUpdate);

            Assert.IsAssignableFrom<BadRequestResult>(actionResult);
        }

        [Fact]
        public async Task UpdateContact_ContactDoesNotExist()
        {
            ContactDto contactForUpdate = new ContactDto()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                ContactPhones = new List<ContactPhoneDto>()
                {
                    new ContactPhoneDto()
                    {
                        ContactPhoneId = 1,
                        ContactId = 1,
                        CountryCode = "30",
                        AreaCode = "478",
                        PhoneNumber = "123456"
                    }
                }
            };

            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.ContactExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.UpdateContact(1, contactForUpdate);

            Assert.IsAssignableFrom<BadRequestResult>(actionResult);
        }

        [Fact]
        public async Task UpdateContact_SuccessfulUpdate()
        {
            Contact updatedContact = new Contact()
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
            };

            ContactDto contactForUpdate = new ContactDto()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                ContactPhones = new List<ContactPhoneDto>()
                {
                    new ContactPhoneDto()
                    {
                        ContactPhoneId = 1,
                        ContactId = 1,
                        CountryCode = "30",
                        AreaCode = "478",
                        PhoneNumber = "123456"
                    }
                }
            };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Contact>(It.IsAny<ContactDto>()))
             .Returns(updatedContact);

            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.ContactExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            repositoryMock
              .Setup(m => m.UpdateContact(It.IsAny<Contact>()));

            repositoryMock
                .Setup(m => m.SaveAsync());

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.UpdateContact(1, contactForUpdate);

            Assert.IsAssignableFrom<NoContentResult>(actionResult);
        }

        [Fact]
        public async Task GetContactAndPhone_FoundMatchingElement()
        {
            Contact contactFromRepo = new Contact()
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
            };

            ContactDto returnedContact = new ContactDto()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                ContactPhones = new List<ContactPhoneDto>()
                {
                    new ContactPhoneDto()
                    {
                        ContactPhoneId = 1,
                        ContactId = 1,
                        CountryCode = "30",
                        AreaCode = "478",
                        PhoneNumber = "123456"
                    }
                }
            };

            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.ContactExistsAsync(1))
                .ReturnsAsync(true);

            repositoryMock
                .Setup(m => m.GetContactPhone(1, 1))
                .ReturnsAsync(contactFromRepo.ContactPhones.First());

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ContactDto>(It.IsAny<ContactDto>()))
                .Returns(returnedContact);

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.GetContactAndPhone(1, 1);

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result.Value);
            Assert.Equal(returnedContact, result.Value);
        }

        [Fact]
        public async Task GetContactAndPhone_NotFoundMatchingContactId()
        {
            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.ContactExistsAsync(1))
                .ReturnsAsync(false);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ContactDto>(It.IsAny<ContactDto>()));

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.GetContactAndPhone(1, 1);

            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetContactAndPhone_NotFoundMatchingContactPhoneId()
        {
            var repositoryMock = new Mock<IPhoneBookRepository>();
            repositoryMock
                .Setup(m => m.ContactExistsAsync(1))
                .ReturnsAsync(true);

            repositoryMock
                .Setup(m => m.GetContactPhone(1, 1))
                .ReturnsAsync((ContactPhone)null);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ContactDto>(It.IsAny<ContactDto>()));

            var controller = new ContactsController(repositoryMock.Object, mapperMock.Object);
            var actionResult = await controller.GetContactAndPhone(1, 1);

            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }
    }
}
