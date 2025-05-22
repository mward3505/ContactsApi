using Microsoft.EntityFrameworkCore;
using ContactsApi.Models;

namespace ContactsApi.Data;

public class ContactsDbContext : DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; } = null!;
}