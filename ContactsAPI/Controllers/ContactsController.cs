using ContactsAPI.Data;
using ContactsAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;


namespace ContactsAPI.Controllers
{
    
    
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactApiDbContext _dbcontext;

        public ContactsController(ContactApiDbContext dbContext)
        {
            this._dbcontext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await _dbcontext.Contacts.ToListAsync());
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContacts([FromRoute] Guid id)
        {
            var contact = await _dbcontext.Contacts.FindAsync(id);


            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }


        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                FullName = addContactRequest.FullName,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone,
                Address = addContactRequest.Address,
            };

            await _dbcontext.Contacts.AddAsync(contact);
            await _dbcontext.SaveChangesAsync();

            return Ok(contact);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await _dbcontext.Contacts.FindAsync(id);


            if (contact != null)
            {
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;
                contact.Address = updateContactRequest.Address;
                contact.FullName = updateContactRequest.FullName;

                await _dbcontext.SaveChangesAsync();
                return Ok(contact);
            }

            return NotFound();

        }

        
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await _dbcontext.Contacts.FindAsync(id);

            if (contact != null)
            {
                _dbcontext.Remove(contact);

                await _dbcontext.SaveChangesAsync();
                return Ok(contact);

            }

            return NotFound();
        }



    }
}
