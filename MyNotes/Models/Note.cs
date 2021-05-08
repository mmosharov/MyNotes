using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;


namespace MyNotes.Models
{

    [Microsoft.EntityFrameworkCore.Index(nameof(UserId))]
    public class Note
    {

        private MyNotesDbContext db;

        public Note(MyNotesDbContext context)
        {
            db = context;
        }


        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }

        public List<User> SharedToUser()
        {

            return (from noteSharing in db.NotesSharings
                   join user in db.Users on noteSharing.ShareWithUserId equals user.Id
                   where noteSharing.NoteId == this.Id
                   select user).ToList();

        }
    }

}