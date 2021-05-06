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

        public List<int> SharedToUser()
        {
            return db.NotesSharings.Where(r => r.NoteId == Id).Select(r => r.ShareWithUserId).ToList();
        }
    }

}