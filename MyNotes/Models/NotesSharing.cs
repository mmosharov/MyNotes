using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyNotes.Models
{

    [Microsoft.EntityFrameworkCore.Index(nameof(ShareWithUserId))]
    public class NotesSharing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NoteId { get; set; }
        
        public int ShareWithUserId { get; set; }

    }
}