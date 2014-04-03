using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers.Views
{
    [Authorize(Roles = "Tenant Manager, Manager, User")]
    public class UserController : Controller
    {
        //
        // GET: /User/

        private PMEntities db = new PMEntities();

        public ActionResult Index(string tenant)
        {
            if (db.Personnels.Where(x => x.Id == User.Identity.Name).Single().Tenant.Name.ToLower() == tenant.ToLower())
            {
                return View(db.Tenants.Where(x => x.Name == tenant).Single());
            }
            return RedirectToAction("NotFound", "Error");
        }

    }
}
