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
using System.IO;

namespace ECommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

        
        public ActionResult Index()
        {
            var companies = db.Companies;
            return View(companies.ToList());
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

       
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                if (company.LogoFile != null)
                {
                    company.Logo = UploadFile(company.LogoFile, "Images/Company", company.CompanyId.ToString());
                }
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities.Where(c => c.DepartmentId == company.DepartmentId), "CityId", "Name", company.CityId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", company.DepartmentId);
            return View(company);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities.Where(c => c.DepartmentId == company.DepartmentId), "CityId", "Name", company.CityId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", company.DepartmentId);
            return View(company);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.LogoFile != null)
                {
                    company.Logo = UploadFile(company.LogoFile, "Images/Company", company.CompanyId.ToString());
                }
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities.Where(c => c.DepartmentId == company.DepartmentId), "CityId", "Name", company.CityId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", company.DepartmentId);
            return View(company);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
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
