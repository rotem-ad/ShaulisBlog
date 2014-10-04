using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShaulisBlog.Models;
using ShaulisBlog.DAL;

namespace ShaulisBlog.Controllers
{
    public class PlacesController : Controller
    {
        private BlogDBContext db = new BlogDBContext();

        //
        // GET: /Places/
        /*
        * Method which handles map display in Index view
        */

        public ActionResult Index()
        {
            return View(db.Locations.ToList());
        }

        //
        // GET: /Places/ManagePlaces
        /*
        * Method which populates ManagePlaces view with list of all places
        */

        public ActionResult ManagePlaces()
        {
            return View(db.Locations.ToList());
        }

        //
        // GET: /Places/Details/5

        public ActionResult Details(int id = 0)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // GET: /Places/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Places/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("ManagePlaces");
            }

            return View(location);
        }

        //
        // GET: /Places/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Places/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManagePlaces");
            }
            return View(location);
        }

        //
        // GET: /Places/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Places/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
            db.SaveChanges();
            return RedirectToAction("ManagePlaces");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}