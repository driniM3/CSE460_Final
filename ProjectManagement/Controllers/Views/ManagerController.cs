using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers.Views
{
    [Authorize(Roles = "Tenant Manager, Manager")]
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/
        private PMEntities db = new PMEntities();

        public ActionResult Index(string tenant)
        {
            if (db.Personnels.Where(x => x.Id == User.Identity.Name).Single().Tenant.Name.ToLower() == tenant.ToLower())
            {
                Tenant te = db.Tenants.Where(x => x.Name == tenant).Single();
                var Manager = db.Personnels.Where(x => x.Id == User.Identity.Name).Single();
                ViewBag.Tenant = te;
                ViewBag.Users = te.Personnels.Where(x => x.Type == "User");
                return View(Manager);
            }
            return RedirectToAction("NotFound", "Error");
        }

        public ActionResult Project(string tenant, int projectId)
        {
            if (db.Personnels.Where(x => x.Id == User.Identity.Name).Single().Tenant.Name.ToLower() == tenant.ToLower())
            {
                var project = db.Projects.Where(x => x.Id == projectId).Single();
                return View(project);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProjectStatus(String TenantId, String ProjectName, String NewStatus)
        {
            return RedirectToAction("Index", new { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProject(String TenantId, String Name, String StartDateDay, String StartDateMonth, String StartDateYear, String EndDateDay, String EndDateMonth, String EndDateYear, String status, String managerId, IEnumerable<String> users)
        {

            DateTime StartDate = Convert.ToDateTime(String.Format("{0}/{1}/{2}", StartDateMonth, StartDateDay, StartDateYear));
            DateTime EndDate = Convert.ToDateTime(String.Format("{0}/{1}/{2}", EndDateMonth, EndDateDay, EndDateYear));

            var project = new Project
            {
                TenantId = Convert.ToInt32(TenantId),
                Name = Name,
                StartDate = StartDate,
                ExpectedEndDate = EndDate,
                Status = status,
                Manager = managerId
            };

            db.Projects.Add(project);
            db.SaveChanges();

            return RedirectToAction("Index", new { });
        }

    }
}
