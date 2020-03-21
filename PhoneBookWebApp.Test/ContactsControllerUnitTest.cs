using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PhoneBookWebApp.Controllers;
using PhoneBookWebApp.Entities;
using PhoneBookWebApp.Models;
using PhoneBookWebApp.Repository;
using System;
using System.Collections.Generic;
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
    }
}
