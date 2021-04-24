using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNotes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyNotes.Controllers
{
    public class Notes : Controller
    {
        private MyNotesDbContext db;

        public Notes(MyNotesDbContext context)
        {
            db = context;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            
            ViewBag.count = db.Notes.Count();

            return View(await db.Notes.ToListAsync());

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
                return View(note);
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

        //// GET: HomeController1/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: HomeController1/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: HomeController1/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: HomeController1/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: HomeController1/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: HomeController1/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
