using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ECommerce.Models;
using static ECommerce.Models.Helper.FileHelper;
using static ECommerce.Models.Helper.UserHelper;

namespace ECommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ECommerceContext db = new ECommerceContext();


        public ActionResult Index()
        {
            var users = db.Users;
            return View(users.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name");
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                CreateUserASP(user.UserName, "User");
                if (user.PhotoFile != null)
                {
                    user.Photo = UploadFile(user.PhotoFile, "Images/User", user.UserId.ToString());
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", user.DepartmentId);
            return View(user);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            TempData["UserName"] = user.UserName;
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", user.DepartmentId);
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.PhotoFile != null)
                {
                    user.Photo = UploadFile(user.PhotoFile, "Images/User", user.UserId.ToString());
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                UpdateUserName(TempData["UserName"].ToString(), user.UserName);
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", user.DepartmentId);
            return View(user);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            DeleteUser(user.UserName);
            return RedirectToAction("Index");
        }


        public JsonResult GetCities(int DepartmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var Cities = db.Cities.Where(c => c.DepartmentId == DepartmentId).ToList();
            return Json(Cities, JsonRequestBehavior.AllowGet);
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
