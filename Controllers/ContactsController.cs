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
    public async Task<ActionResult<List<Contact>>> GetAllContacts(
        [FromQuery] string? search,
        [FromQuery] string? sortBy,
        [FromQuery] string? order)
    {
        IQueryable<Contact> query = _db.Contacts;

        // Filtering
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c =>
                c.FirstName.Contains(search) ||
                c.LastName.Contains(search) ||
                c.Email.Contains(search));
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            bool ascending = string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase);

            query = sortBy.ToLower() switch
            {
                "firstname" => ascending ? query.OrderBy(c => c.FirstName) : query.OrderByDescending(c => c.FirstName),
                "lastname" => ascending ? query.OrderBy(c => c.LastName) : query.OrderByDescending(c => c.LastName),
                "email" => ascending ? query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email),
                _ => query // fallback to no srort if invalid field
            };
        }

        return Ok(await query.ToListAsync());
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