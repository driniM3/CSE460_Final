using ProjectManagement.Filters;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers.Views
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index(string tenant)
        {
            string user = User.Identity.Name;
            var db = new PMEntities();
            if (User.Identity.IsAuthenticated && (db.Personnels.Find(user).Tenant.Name.ToLower() == tenant.ToLower() || tenant == "New"))
            {
                tenant = db.Personnels.Find(user).Tenant.Name;
                if (User.IsInRole("Tenant Manager"))
                {
                    return RedirectToAction("Index", "TenantManagement", new { tenant = tenant });
                }
                else if (User.IsInRole("Manager"))
                {
                    return RedirectToAction("Index", "Manager", new { tenant = tenant });
                }
                else if (User.IsInRole("User"))
                {
                    return RedirectToAction("Index", "User", new { tenant = tenant });
                }
                else
                {
                    return View(db.Tenants.ToList());
                }

            }
            else if (!User.Identity.IsAuthenticated && tenant == "New")
            {
                return View(db.Tenants.ToList());
            }
            else if (!User.Identity.IsAuthenticated && db.Tenants.Any(x=>x.Name.ToLower() == tenant.ToLower()))
            {
                return RedirectToAction("LogIn", "Account", new { tenant = db.Tenants.Where(x => x.Name.ToLower() == tenant.ToLower()).Single().Name });
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

    }
}
