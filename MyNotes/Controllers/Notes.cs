using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNotes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using Microsoft.Extensions.Logging;


namespace MyNotes.Controllers
{
    public class Notes : Controller
    {
        private MyNotesDbContext db;
        private readonly ILogger _logger;

        public Notes(MyNotesDbContext context, ILogger<Notes> logger)
        {
            db = context;
            _logger = logger;
        }

        [Authorize]
        public async Task<ActionResult> Index(int? page)
        {

            _logger.LogInformation("test");

            var currentUserId = int.Parse(User.Identity.Name);

            var own = from note in db.Notes
                      where note.UserId == currentUserId
                      select new NoteForView() { Note = note, SharingType = NoteSharingType.Own };
            var shared = from note in db.Notes
                         join notesSharing in db.NotesSharings on note.Id equals notesSharing.NoteId
                         where notesSharing.ShareWithUserId == currentUserId && note.UserId != currentUserId
                         select new NoteForView() { Note = note, SharingType = NoteSharingType.Shared };
            var all = await own.Union(shared).OrderByDescending(n => n.Note.Created).ToListAsync();

            int pageNumber = (page ?? 1);
            int pageSize = 10;

            ViewBag.count = all.Count();

            return View(all.ToPagedList(pageNumber, pageSize));

        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {

            Note note = db.Notes.Where(i => i.Id == id).FirstOrDefault();
            if (note is null)
            {
                return NotFound();
            }
            else
            {
                if (note.UserId == int.Parse(User.Identity.Name))
                {

                    var users = db.Users.ToList();
                    ViewBag.users = users;

                    var model = new NoteForView() { Note = note, SharedToUsers = note.SharedToUsers()};
                    return View(model);
                }
                else
                {
                    return StatusCode(403);
                }
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit(Note note)
        {

            Note db_note = db.Notes.Where(i => i.Id == note.Id).FirstOrDefault();
            if (db_note is null)
            {
                return NotFound();
            }
            else
            {
                db_note.Text = note.Text;
                db.Notes.Update(db_note);
                await db.SaveChangesAsync();
                return Redirect("~/notes");
            }

        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(Note note)
        {

            note.Created = DateTime.Now;
            note.UserId = int.Parse(User.Identity.Name);
            db.Notes.Add(note);
            await db.SaveChangesAsync();
            return Redirect("~/notes");

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {

            Note note = db.Notes.Where(i => i.Id == id).FirstOrDefault();
            if (note is null)
            {
                return NotFound();
            }
            else
            {
                note.Text = note.Text;
                db.Notes.Remove(note);
                await db.SaveChangesAsync();
                return Ok();
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SaveSharedToUsers(int noteId, List<int> newSharedToUsers)
        {
            Note note = db.Notes.Where(i => i.Id == noteId).FirstOrDefault();
            if (note is null)
            {
                return NotFound();
            }
            else
            {

                var noteSharings = await (from noteSharing in db.NotesSharings
                                         where noteSharing.NoteId == noteId
                                         select noteSharing).ToListAsync();

                foreach (var noteSharing in noteSharings)
                {
                    db.NotesSharings.Remove(noteSharing);
                }
                foreach (int userId in newSharedToUsers)
                {
                    db.NotesSharings.Add(new NotesSharing(){ NoteId = noteId, ShareWithUserId = userId });
                }

                await db.SaveChangesAsync();

                return Ok();

            }
        }

    }
}
