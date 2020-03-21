﻿using System;
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
    }
}