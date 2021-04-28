using Microsoft.EntityFrameworkCore;

namespace MyNotes.Models
{
    public class MyNotesDbContext : DbContext
    {

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserMobileVerification> UserMobileVerifications { get; set; }

        public MyNotesDbContext(DbContextOptions<MyNotesDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

    }

}