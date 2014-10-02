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
    public class FanClubController : Controller
    {
        private BlogDBContext db = new BlogDBContext();

        //
        // GET: /FanClub/

        public ActionResult Index()
        {
            return View(db.Fans.ToList());
        }

        //
        // POST: /FanClub/FilterFans
        /*
        * Method which handles fans searches from Index view
        */
        [HttpPost]
        public ActionResult FilterFans(string firstName, int minAge = 0, int minSeniority = 0)
        {
            IEnumerable<Fan> filteredFans = db.Fans; // Holds the result set

            // If "First Name" was given as input parameter
            if (!String.IsNullOrWhiteSpace(firstName)) 
                filteredFans = filteredFans.Where(f => f.FName.ToLower().Contains(firstName.ToLower()));
            // If "Minimum Age" was given as input parameter
            if (minAge != 0)
                filteredFans = filteredFans.Where(f => (DateTime.Today.Year - f.BDate.Year) >= minAge );
            // If "Minimum Seniority" was given as input parameter
            if (minSeniority != 0)
                filteredFans = filteredFans.Where(f => f.Seniority >= minSeniority);

            // Make sure to return list with distinct values to avoid duplicate fans in the view
            return View("Index", filteredFans.ToList().Distinct());
        }

        //
        // GET: /FanClub/Details/5

        public ActionResult Details(int id = 0)
        {
            Fan fan = db.Fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        //
        // GET: /FanClub/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FanClub/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fan fan)
        {
            if (ModelState.IsValid)
            {
                db.Fans.Add(fan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fan);
        }

        //
        // GET: /FanClub/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Fan fan = db.Fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        //
        // POST: /FanClub/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fan fan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fan);
        }

        //
        // GET: /FanClub/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Fan fan = db.Fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        //
        // POST: /FanClub/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fan fan = db.Fans.Find(id);
            db.Fans.Remove(fan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}