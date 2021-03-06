﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Politiq.Models.ObjectModel;
using Politiq.Models;
using Politiq.Models.ObjectManager;

namespace Politiq.Controllers
{ 
    public class BillController : Controller
    {
        private DAL db = new DAL();

        //
        // GET: /Bill/

        public ViewResult Index()
        {
            return View(db.Legislations.ToList());
        }

        //
        // GET: /Bill/Details/5

        public ViewResult Details(int id)
        {
            Legislation legislation = db.Legislations.Find(id); 
            ViewBag.Style = LegislationManager.GenerateStyle(legislation);
            return View(legislation);
        }

        //
        // GET: /Bill/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Bill/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NewLegislationView legislation)
        {
            legislation.OriginatingChamber = 1;    // House of Commons by default, hard-coded. Change if Senate included.

            if (ModelState.IsValid)
            {
                LegislationManager billManager = new LegislationManager();
                int savedLegislation = billManager.Save(legislation);
                if (savedLegislation == 0)
                {
                    ModelState.AddModelError("", "Parliament is not in session.");
                }
                else
                {
                    return RedirectToAction("Create", "Provision", new { bill = savedLegislation });
                }   
            }
            return View(legislation);
        }
        
        //
        // GET: /Bill/Edit/5
 
        public ActionResult Edit(int id)
        {
            Legislation legislation = db.Legislations.Find(id);
            return View(legislation);
        }

        //
        // POST: /Bill/Edit/5

        [HttpPost]
        public ActionResult Edit(Legislation legislation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(legislation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(legislation);
        }

        //
        // GET: /Bill/Delete/5
 
        public ActionResult Delete(int id)
        {
            Legislation legislation = db.Legislations.Find(id);
            return View(legislation);
        }

        //
        // POST: /Bill/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Legislation legislation = db.Legislations.Find(id);
            db.Legislations.Remove(legislation);
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