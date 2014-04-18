using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ProjectManagement.Filters;
using ProjectManagement.Models;


namespace ProjectManagement.Controllers.Views
{
    [Authorize(Roles="Tenant Manager")]
    public class TenantManagementController : Controller
    {
        // GET: /TenantManagement/
        private PMEntities db = new PMEntities();
        
        public ActionResult Index(string tenant)
        {
            if (db.Personnels.Where(x => x.Id == User.Identity.Name).Single().Tenant.Name.ToLower() == tenant.ToLower())
            {
                var t = db.Tenants.Where(x => x.Name == tenant).Single();
                return View(t);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTenant(Tenant model)
        {
            var tenant = db.Tenants.Where(x => x.Id == model.Id).Single();
            tenant.Name = model.Name;
            tenant.Style = model.Style;
            db.Tenants.Attach(tenant);
            db.Entry(tenant).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { tenant = tenant.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(String tenantId, string username, string password, string passwordConfirm, string type)
        {

            if (password == passwordConfirm)
            {
                int Id = Convert.ToInt32(tenantId);
                var tenant = db.Tenants.Where(x => x.Id == Id).Single();

                WebSecurity.CreateUserAndAccount(username, password);
                System.Web.Security.Roles.AddUserToRole(username, type);

                var person = new Personnel
                {
                    Id = username,
                    Name = username,
                    TenantId = Id,
                    Type = type
                };

                db.Personnels.Add(person);
                db.SaveChanges();

                tenant.Personnels.Add(person);
                db.Tenants.Attach(tenant);
                db.Entry(tenant).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { });
            }

            return RedirectToAction("NotFound", "Error");

        }

        public ActionResult RemoveUser(string username, string tenantId)
        {
            if (username != null && tenantId != null)
            {
                int Id = Convert.ToInt32(tenantId);
                var tenant = db.Tenants.Where(x => x.Id == Id).Single();

                Personnel personToRemove = null;
                foreach (Personnel person in tenant.Personnels)
                {
                    if (person.Name.Equals(username))
                    {
                        personToRemove = person;
                        break;
                    }
                }

                if (personToRemove != null)
                {

        //            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(username);
        //            ((SimpleMembershipProvider)Membership.Provider).DeleteUser(username, true);

                    db.Personnels.Remove(personToRemove);
                    db.Entry(personToRemove).State = System.Data.EntityState.Deleted;
                    db.SaveChanges();

                    tenant.Personnels.Remove(personToRemove);
                    db.Tenants.Attach(tenant);
                    db.Entry(tenant).State = System.Data.EntityState.Modified;
                    db.SaveChanges();


                }

            }
            return RedirectToAction("Index", new { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLogo(string tenantId, string logoUrl)
        {
            if (tenantId != null && logoUrl != null)
            {
                int Id = Convert.ToInt32(tenantId);
                var tenant = db.Tenants.Where(x => x.Id == model.Id).Single();
            }
            return RedirectToAction("Index", new { });
        }
    }
}
