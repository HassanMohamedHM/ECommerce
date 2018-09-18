using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ECommerce.Models;

namespace ECommerce.Controllers
{
    [Authorize(Roles ="User")]
    public class TaxesController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

        public ActionResult Index()
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var taxes = db.Taxes.Where(t => t.CompanyId == user.CompanyId);
            return View(taxes.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }
        
        public ActionResult Create()
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var tax = new Tax() {CompanyId=user.CompanyId };
            return View(tax);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tax tax)
        {
            if (ModelState.IsValid)
            {
                db.Taxes.Add(tax);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(tax);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Tax tax)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tax).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tax);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tax tax = db.Taxes.Find(id);
            db.Taxes.Remove(tax);
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
