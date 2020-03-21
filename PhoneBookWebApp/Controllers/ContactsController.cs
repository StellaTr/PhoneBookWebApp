using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBookWebApp.Entities;
using PhoneBookWebApp.Models;
using PhoneBookWebApp.Repository;

namespace PhoneBookWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IPhoneBookRepository _repository;
        private readonly IMapper _mapper;

        public ContactsController(IPhoneBookRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDto>> GetContact(int id)
        {
            try
            {
                Contact contactFromRepo = await _repository.GetContact(id);

                if (contactFromRepo == null)
                {
                    return NotFound();
                }

                ContactDto contact = _mapper.Map<ContactDto>(contactFromRepo);

                return Ok(contact);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts(string searchQuery = "")
        {
            try
            {
                IEnumerable<Contact> contactsFromRepo;

                if (searchQuery.Any(c => char.IsLetter(c)))
                {
                    contactsFromRepo = await _repository.SearchContactsByNameAsync(searchQuery);
                }
                else
                {
                    string searchQueryWithoutSpaces = new string(searchQuery.Where(c => !char.IsWhiteSpace(c)).ToArray());
                    string modifiedQuery = Regex.Replace(searchQueryWithoutSpaces, "^([+])", "");

                    contactsFromRepo = await _repository.SearchContactsByPhoneAsync(modifiedQuery);
                }

                IEnumerable<ContactDto> contacts = _mapper.Map<IEnumerable<ContactDto>>(contactsFromRepo);

                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{contactId}/contactphones/{contactPhoneId}")]
        public async Task<ActionResult<ContactDto>> GetContactAndPhone(int contactId, int contactPhoneId)
        {
            try
            {
                bool contactExists = await _repository.ContactExistsAsync(contactId);

                if (!contactExists)
                {
                    return NotFound();
                }

                var contactPhoneFromRepo = await _repository.GetContactPhone(contactId, contactPhoneId);

                if (contactPhoneFromRepo == null)
                {
                    return NotFound();
                }

                ContactDto contactDto = _mapper.Map<ContactDto>(contactPhoneFromRepo.Contact);

                return Ok(contactDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ContactDto>> PostContact(ContactEntry contact)
        {
            try
            {
                if (contact == null)
                {
                    return BadRequest();
                }

                Contact contactFromRepo = _mapper.Map<Contact>(contact);

                _repository.AddContact(contactFromRepo);

                await _repository.SaveAsync();

                Contact postedContact = await _repository.FindAsync(contactFromRepo.ContactPhones.First().ContactId);

                ContactDto contactDto = _mapper.Map<ContactDto>(postedContact);

                return CreatedAtAction("GetContact", new { id = contactDto.ContactId }, contactDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, ContactDto contact)
        {
            try
            {
                if (id != contact.ContactId)
                {
                    return BadRequest();
                }

                bool contactExists = await _repository.ContactExistsAsync(id);

                if (!contactExists)
                {
                    return BadRequest();
                }

                Contact contactFromRepo = _mapper.Map<Contact>(contact);

                _repository.UpdateContact(contactFromRepo);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}