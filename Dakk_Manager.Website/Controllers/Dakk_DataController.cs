using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net.Mail;
using PagedList;
using PagedList.Mvc;
using Dakk_Manager.Website.Models;

namespace Dakk_Manager.Website.Controllers
{
    [Authorize]
    public class Dakk_DataController : Controller
    {
        private DMContext db = new DMContext();

        //public static void SendEmail(string emailbody)
        //{
        //    //Specify the from and to email address
        //    MailMessage mailMessage = new MailMessage
        //        ("centralrecordofficesindh@gmail.com", "centralrecordofficesindh@gmail.com");
        //    //Specify the email body
        //    mailMessage.Body = emailbody + Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        //    //Specify the email Subject
        //    mailMessage.Subject = "Dakks Status";

        //    //No need to specify the SMTP settings as these
        //    // are already specified in web.config
        //    SmtpClient smtpClient = new SmtpClient();
        //    //Finall send the email message using Send() method
        //    smtpClient.Send(mailMessage);
        //}

        // GET: Dakk_Data
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(string SearchString, string uploadDate, string DateonDakk, string P_StatusString, int? pageNumber)
        {

            //string mailbodymessage;
            //mailbodymessage = "You Have " + ViewBag.Pending_Status + " Dakk Pending" + Environment.NewLine + "You Have Seen " + ViewBag.Seen_Status + " Dakks " + Environment.NewLine + "You Have " + ViewBag.Urgent_Status + " Urgent Dakks";
            //SendEmail(mailbodymessage);
            SetStatus_On_Dashboard();

            if (!String.IsNullOrEmpty(uploadDate))
            {
                uploadDate = Convert.ToDateTime(uploadDate).Date.ToString("dd-MM-yyyy");
            }
            if (!String.IsNullOrEmpty(DateonDakk))
            {
                DateonDakk = Convert.ToDateTime(DateonDakk).Date.ToString("dd-MM-yyyy");
            }

            if ((User.IsInRole("Admin") || User.Identity.Name == "secretary") && SearchString != null)
            {
                return View(db.Dakk_Data.Where(x => (((String.IsNullOrEmpty(SearchString) || x.Number.Contains(SearchString) || x.Department.Contains(SearchString)
                                                                     || x.Sectionoforigin.Contains(SearchString) || x.Givennumber.Contains(SearchString) || x.Status.Contains(SearchString) ||
                                                                     x.Subject.Contains(SearchString) || x.ForwardTo.Contains(SearchString)) || x.Receivedby.Contains(SearchString))
                                                                     && (String.IsNullOrEmpty(DateonDakk) || x.DateOnLetter.Contains(DateonDakk))
                                                                     && (String.IsNullOrEmpty(uploadDate) || x.UploadTime.Contains(uploadDate)))).ToList().ToPagedList(pageNumber ?? 1, 10));
            }
            if (User.IsInRole("Admin") || User.Identity.Name == "secretary")
            {
                return View(db.Dakk_Data.ToList().ToPagedList(pageNumber ?? 1, 10));
            }

            if (SearchString != null)
            {

                return View(db.Dakk_Data.Where(x => ((String.IsNullOrEmpty(SearchString) || x.Number.Contains(SearchString) || x.Department.Contains(SearchString)
                                                                  || x.Sectionoforigin.Contains(SearchString) || x.Givennumber.Contains(SearchString) || x.Status.Contains(SearchString) ||
                                                                  x.Subject.Contains(SearchString) || x.ForwardTo.Contains(SearchString))
                                                                  && (x.Receivedby.Contains(User.Identity.Name))
                                                                  && (String.IsNullOrEmpty(DateonDakk) || x.DateOnLetter.Contains(DateonDakk))
                                                                  && (String.IsNullOrEmpty(uploadDate) || x.UploadTime.Contains(uploadDate)))).ToList().ToPagedList(pageNumber ?? 1, 10));

            }

            else
            {
                return View(db.Dakk_Data.Where(x => x.Receivedby.Contains(User.Identity.Name) || x.ForwardTo.Contains(User.Identity.Name)).ToList().ToPagedList(pageNumber ?? 1, 10));
            }

        }

        private void SetStatus_On_Dashboard()
        {
     
           
            if (User.IsInRole("Admin") || User.Identity.Name=="secretary")
            {
                var Check_Status = db.Dakk_Data.ToList();

                Check_Status = db.Dakk_Data.Where(x => x.Status == "Pending").ToList();
                ViewBag.Pending_Status = (Check_Status.Count()).ToString();

                Check_Status = db.Dakk_Data.Where(x => x.Status == "Seen").ToList();
                ViewBag.Seen_Status = (Check_Status.Count()).ToString();

                Check_Status = db.Dakk_Data.Where(x => x.Status == "Urgent").ToList();
                ViewBag.Urgent_Status = (Check_Status.Count()).ToString();

                Check_Status = db.Dakk_Data.Where(x => x.Status == "Objection").ToList();
                ViewBag.Objection_Status = (Check_Status.Count()).ToString();
            }
            else
            {
                var Check_Status1 = db.Dakk_Data.Where(x => x.Receivedby.Contains(User.Identity.Name)).ToList();
                var Check_Status2 = db.Dakk_Data.Where(x => x.Receivedby.Contains(User.Identity.Name)).ToList();
                Check_Status1 = db.Dakk_Data.Where(x => x.Status == "Pending" && (x.Receivedby.Contains(User.Identity.Name))).ToList();
                Check_Status2 = db.Dakk_Data.Where(x => x.Status == "Pending" && (x.ForwardTo.Contains(User.Identity.Name))).ToList();
                ViewBag.Pending_Status = (Check_Status1.Count() + Check_Status2.Count()).ToString();

                Check_Status1 = db.Dakk_Data.Where(x => x.Status == "Seen" && (x.Receivedby.Contains(User.Identity.Name))).ToList();
                Check_Status2 = db.Dakk_Data.Where(x => x.Status == "Seen" && (x.ForwardTo.Contains(User.Identity.Name))).ToList();
                ViewBag.Seen_Status = (Check_Status1.Count() + Check_Status2.Count()).ToString();

                Check_Status1 = db.Dakk_Data.Where(x => x.Status == "Urgent" && (x.Receivedby.Contains(User.Identity.Name))).ToList();
                Check_Status2 = db.Dakk_Data.Where(x => x.Status == "Urgent" && (x.ForwardTo.Contains(User.Identity.Name))).ToList();
                ViewBag.Urgent_Status = (Check_Status1.Count() + Check_Status2.Count()).ToString();

                Check_Status1 = db.Dakk_Data.Where(x => x.Status == "Objection" && (x.Receivedby.Contains(User.Identity.Name))).ToList();
                Check_Status2 = db.Dakk_Data.Where(x => x.Status == "Objection" && (x.ForwardTo.Contains(User.Identity.Name))).ToList();
                ViewBag.Objection_Status = (Check_Status1.Count() + Check_Status2.Count()).ToString();
            }
           
        }

        // GET: Dakk_Data/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dakk_Data dakk_Data = db.Dakk_Data.Find(id);
            if (dakk_Data == null)
            {
                return HttpNotFound();
            }
            return View(dakk_Data);
        }

        // GET: Dakk_Data/Create
        [Authorize(Users = "R&I")]
        public ActionResult Create()
        {
            CreateStaticList();
            return View();
        }

        // POST: Dakk_Data/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize (Users ="R&I")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateOnLetter,DateReceived,Department,Subject,Givennumber,Pages,Addressee,Sectionoforigin,Receivedby,Pdfdirectory,Name,Number,UploadTime,Status")] Dakk_Data dakk_Data, HttpPostedFileBase file, string DakkDate, string ReceivedDate)
        {

            if (ModelState.IsValid)
            {
                db.Dakk_Data.Add(dakk_Data);

                UploadDakk(dakk_Data, file, DakkDate, ReceivedDate);

                db.SaveChanges();
                return RedirectToAction("Create");
            }
            CreateStaticList();
            return View();
        }

        private void UploadDakk(Dakk_Data dakk_Data, HttpPostedFileBase file, string DakkDate, string ReceivedDate)
        {
            dakk_Data.DateOnLetter = Convert.ToDateTime(DakkDate).Date.ToString("dd-MM-yyyy");
            dakk_Data.DateReceived = Convert.ToDateTime(ReceivedDate).Date.ToString("dd-MM-yyyy");

            dakk_Data.UploadTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");

            int getid=0;
            try
            {
                if (file.ContentLength > 0)
                {
                    getid = db.Dakk_Data.Select(x => x.ID).DefaultIfEmpty().Max();
                    string _FileName = Path.GetFileName(file.FileName);
                    getid++;
                  
                    string extension = Path.GetExtension(file.FileName);
                    _FileName = getid + "_" + dakk_Data.Sectionoforigin + "_" + dakk_Data.Status + "_" + dakk_Data.DateOnLetter + extension;
                    string _path = Path.Combine(Server.MapPath("~/Downloads"), _FileName);
                    string uploadedfilename;
                    uploadedfilename = _path;
                    dakk_Data.Pdfdirectory = _FileName;

                    file.SaveAs(uploadedfilename);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        // GET: Dakk_Data/Edit/5
        public ActionResult Edit(int? id)
        {
            CreateStaticList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dakk_Data dakk_Data = db.Dakk_Data.Find(id);
            if (dakk_Data == null)
            {
                return HttpNotFound();
            }
            return View(dakk_Data);
        }

        // POST: Dakk_Data/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateOnLetter,DateReceived,Department,Subject,Givennumber,Pages,Addressee,Sectionoforigin,Pdfdirectory,Receivedby,Number,UploadTime,Status,ForwardTo,Comments")] Dakk_Data dakk_Data)
        {

            if (ModelState.IsValid)
            {
                db.Entry(dakk_Data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dakk_Data);
        }

        // GET: Dakk_Data/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dakk_Data dakk_Data = db.Dakk_Data.Find(id);
            if (dakk_Data == null)
            {
                return HttpNotFound();
            }
            return View(dakk_Data);
        }

        // POST: Dakk_Data/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dakk_Data dakk_Data = db.Dakk_Data.Find(id);
            db.Dakk_Data.Remove(dakk_Data);
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

        public ActionResult Download(string dakkname)
        {
            string path = Server.MapPath("~/Downloads");

            byte[] fileBytes = System.IO.File.ReadAllBytes(path + @"\" + dakkname);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, dakkname);
        }
        private void CreateStaticList()
        {
            var Deparment_list = new List<string>() { "Governor Secretariat / House" ,"Chief Minister","Services General Admin. Coord Deptt.","Finance",
            "Investment Department","Planning & Development","Excise & Taxation","Board of Revenue","Home","Law & Parliamentary Affairs",
            "Agriculture Supply & Prices","Food","Livestock & Fisheries", "Cooperation","Irrigation","Energy,Mines & Minerals Development","Industries & Commerce",
            "Labour & Human Resources","Works & Services", "School Education", "Transport and Mass Transit", "Environment, Forest & Wildlife",
            "Local Government & HTP","Housing & Town Planning","Katchi Abadies","Public Health Engineering and RDD","Health","Sports & Youth Affairs",
            "Information & amp; Archives", "Minorities Affairs"," Culture, Tourism and Antiquities Department", "Information Technology", "College Education",
            "Universities and Boards","Dept of Empowerment - Persons Disabilities",  "Population Welfare",  "Women Development",  "Rehabilitation",
            "Social Welfare","Auqaf, Relgious Affairs & Zakat","Human Rights","Inter Provincial Coordination","Provincial Assembly","Provincial Ombudsman",
            "Ombudsman for Protection against woman harassment at workplace","AG Sindh","Federal Govt","Sindh Revenue Board"};
            ViewBag.list = Deparment_list;

            var status_list = new List<string>() { "Pending", "Urgent", "Seen", "Objection" };
            ViewBag.list2 = status_list;

            var forward_to_list = new List<string> {"Planning","Development","General","Admn-I","Admn-II","Health","Project","All Other Sections Will Be Added"};
            ViewBag.list3 = forward_to_list;
        }
    }
}
