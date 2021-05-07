using Microsoft.EntityFrameworkCore;

namespace MyNotes.Models
{
    public class MyNotesDbContext : DbContext
    {

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserMobileVerification> UserMobileVerifications { get; set; }
        public DbSet<NotesSharing> NotesSharings { get; set; }

        public MyNotesDbContext(DbContextOptions<MyNotesDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotesSharing>()
                .HasKey(ns => new { ns.NoteId, ns.ShareWithUserId });
        }

    }

}