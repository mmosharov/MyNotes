using System.Collections.Generic;

namespace MyNotes.Models
{

    public enum NoteSharingType : int
    {
        Own = 0,
        Shared = 1
    }

    public class NoteForView
    {
        public Note Note { get; set; }
        public NoteSharingType SharingType { get; set; }
        public List<int> SharedToUsers { get; set; }
        
    }
}