using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dakk_Manager.Database;
using Dakk_Manager.Entities;

namespace Dakk_Manager.Website.Controllers
{
    public class Dakk_Data_DetailController : Controller
    {
        private DMContext db = new DMContext();

        // GET: Dakk_Data_Detail
        public ActionResult Index()
        {
            return View(db.Dakk_Data_Details.ToList());
        }

        // GET: Dakk_Data_Detail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dakk_Data_Detail dakk_Data_Detail = db.Dakk_Data_Details.Find(id);
            if (dakk_Data_Detail == null)
            {
                return HttpNotFound();
            }
            return View(dakk_Data_Detail);
        }

        // GET: Dakk_Data_Detail/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dakk_Data_Detail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DakkDataDetailId,FilePath,comments,Number,UploadTime,Status")] Dakk_Data_Detail dakk_Data_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Dakk_Data_Details.Add(dakk_Data_Detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dakk_Data_Detail);
        }

        // GET: Dakk_Data_Detail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dakk_Data_Detail dakk_Data_Detail = db.Dakk_Data_Details.Find(id);
            if (dakk_Data_Detail == null)
            {
                return HttpNotFound();
            }
            return View(dakk_Data_Detail);
        }

        // POST: Dakk_Data_Detail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DakkDataDetailId,FilePath,comments,Number,UploadTime,Status")] Dakk_Data_Detail dakk_Data_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dakk_Data_Detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dakk_Data_Detail);
        }

        // GET: Dakk_Data_Detail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dakk_Data_Detail dakk_Data_Detail = db.Dakk_Data_Details.Find(id);
            if (dakk_Data_Detail == null)
            {
                return HttpNotFound();
            }
            return View(dakk_Data_Detail);
        }

        // POST: Dakk_Data_Detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dakk_Data_Detail dakk_Data_Detail = db.Dakk_Data_Details.Find(id);
            db.Dakk_Data_Details.Remove(dakk_Data_Detail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
