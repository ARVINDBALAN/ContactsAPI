using ContactsAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Data
{
    public class ContactApiDbContext : DbContext
    {
        public ContactApiDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Users> User { get; set; }
    }
}
