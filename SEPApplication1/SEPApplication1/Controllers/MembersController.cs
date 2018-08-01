using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEPApplication1.Models;

namespace SEPApplication1.Controllers
{
    public class MembersController : Controller
    {
        //
        // GET: /Members/
        private SEPEntities db = new SEPEntities();
        private API api = new API();
        public ActionResult Index(int courseId)
        {
            ViewBag.CourseId = courseId;
            var members = db.Members.Where(m => m.Course_id == courseId).ToList();
            return View(members);
        }
        // GET: /Session/
        //public ActionResult Index(int id)
        //{
        //    ViewBag.CourseId = id;
        //    ViewBag.courseCode = db.Courses.FirstOrDefault(x => x.id == id).Code;
        //    var member = db.Members.Where(s => s.Course_id == id);
        //    return View(member.ToList());
        //}

        // GET: /Session/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member mb = db.Members.Find(id);
            if (mb == null)
            {
                return HttpNotFound();
            }
            return View(mb);
        }

        // GET: /Session/Create
        public ActionResult Create(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        // POST: /Session/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int courseId, Member member)
        {
            var result = new API().GetStudent(member.Code);
            if (result.Code == 0)
            {
                member.Fullname = result.Data.Fullname;
                member.Birthday = result.Data.Birthday;
                member.Course_id = courseId;
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index", new { courseId = courseId });
            }
            //session.Course_id = courseId;
            //string uid = Session["ID"].ToString();
            //string secret = Session["Secret"].ToString();
            //string course = db.Courses.FirstOrDefault(x => x.id == courseId).id.ToString();

            //if (ModelState.IsValid)
            //{
            //    db.Members.Add(session);
            //    db.SaveChanges();
            //    var a = api.postmember(uid, secret, db.Courses.FirstOrDefault(x => x.id == courseId).Code);
            //    return RedirectToAction("Index", new { courseId = courseId });
            //}
            ViewBag.CourseId = courseId;

            return View(member);
            //session.Course_id = courseId;
            //string uid = Session["ID"].ToString();
            //string secret = Session["Secret"].ToString();
            //string course = db.Courses.FirstOrDefault(x => x.id == courseId).id.ToString();

            //if (ModelState.IsValid)
            //{
            //    db.Members.Add(session);
            //    db.SaveChanges();
            //    var a = api.postmember(uid, secret, db.Courses.FirstOrDefault(x => x.id == courseId).Code);
            //    return RedirectToAction("Index", new { id = courseId });
            //}
            //ViewBag.Course_id = courseId;
            //return View(session);
        }

        public ActionResult SyncMember(string id)
        {
            var course = db.Courses.FirstOrDefault(x => x.Code == id);
            var member = api.GetMember(id);
            Member Nmember = new Member();
            foreach (var item in member.Data)
            {
                Nmember.Course_id = course.id;
                Nmember.Birthday = item.birthday;
                Nmember.Fullname = item.fullname;
                Nmember.Code = item.ID;
                db.Members.Add(Nmember);
                db.SaveChanges();
            }

            return RedirectToAction("Index", new { id = course.id });
        }

        // GET: /Session/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Member session = db.Members.Find(id);
        //    if (session == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Course_id = new SelectList(db.Courses, "id", "Code", session.Course_id);
        //    return View(session);
        //}

        //// POST: /Session/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,Date,Info,Course_id")] Session session)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(session).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Course_id = new SelectList(db.Courses, "id", "Code", session.Course_id);
        //    return View(session);
        //}

        //// GET: /Session/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Session session = db.Sessions.Find(id);
        //    if (session == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(session);
        //}

        //// POST: /Session/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Session session = db.Sessions.Find(id);
        //    db.Sessions.Remove(session);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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