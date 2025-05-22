using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsApi.Models;
using ContactsApi.Data;

namespace ContactsApi.Controllers;

[ApiController]
[Route("contacts")]
public class ContactsController : ControllerBase
{
    private readonly ContactsDbContext _db;

    public ContactsController(ContactsDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Contact>>> GetAllContacts()
    {
        return Ok(await _db.Contacts.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContactById(Guid id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        return contact == null ? NotFound() : Ok(contact);
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> CreateContact(Contact newContact)
    {
        newContact.Id = Guid.NewGuid();
        _db.Contacts.Add(newContact);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllContacts), new { id = newContact.Id }, newContact);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateContact(Guid id, Contact updatedContact)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact == null) return NotFound();

        contact.FirstName = updatedContact.FirstName;
        contact.LastName = updatedContact.LastName;
        contact.Email = updatedContact.Email;
        contact.Phone = updatedContact.Phone;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteContact(Guid id)
    {
        var contact = _db.Contacts.Find(id);
        if (contact == null)
        {
            return NotFound();
        }

        _db.Contacts.Remove(contact);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}