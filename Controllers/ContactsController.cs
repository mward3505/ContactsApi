using Microsoft.AspNetCore.Mvc;
using ContactsApi.Models;

namespace ContactsApi.Controllers;

[ApiController]
[Route("contacts")]
public class ContactsController : ControllerBase
{
    // Fake in-memory storage
    private static readonly List<Contact> contacts = new();

    [HttpGet]
    public ActionResult<List<Contact>> GetAllContacts()
    {
        return Ok(contacts);
    }

    [HttpPost]
    public ActionResult<Contact> CreateContact(Contact newContact)
    {
        newContact.Id = Guid.NewGuid();
        contacts.Add(newContact);
        return CreatedAtAction(nameof(GetAllContacts), new { id = newContact.Id }, newContact);
    }

}