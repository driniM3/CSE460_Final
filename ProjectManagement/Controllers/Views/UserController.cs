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
                Tenant ten = db.Tenants.Where(x => x.Name == tenant).Single();
                ViewBag.Username = User.Identity.Name;
                ViewBag.UserProjects = db.Projects.Where(x => x.ProjectPersonnels.Any(m => m.PersonnelId == User.Identity.Name)).ToList();
                ViewBag.UserRequirements = db.Requirements.Where(x => x.Personnel.Name == User.Identity.Name).ToList();
                return View(ten);
            }
            return RedirectToAction("NotFound", "Error");
        }

        public ActionResult EditRequirementStatus(String requirementId, String newStatus)
        {
            int requirementIdInt = Convert.ToInt32(requirementId);
            Requirement requirement = db.Requirements.Where(x => x.Id == requirementIdInt).Single();

            requirement.Status = newStatus;
            db.Entry(requirement).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { });
        }

    }
}
