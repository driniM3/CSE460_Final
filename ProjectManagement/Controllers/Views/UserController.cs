using ProjectManagement.DataProviders;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers.Views
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        //
        // GET: /User/

        private PMEntities db = new PMEntities();

        public ActionResult Index(string tenant)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name))
            {
                return View(db.Personnels.Where(x => x.Id == User.Identity.Name).Single());
            }
            else return RedirectToAction("NotFound", "Error");
        }

        public ActionResult Requirement(string tenant, int requirementId)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name)
                && SecurityProvider.userAuthFromRequirement(requirementId, User.Identity.Name))
            {
                var r = db.Requirements.Where(x => x.Id == requirementId).Single();
                return View(r);
            }
            else return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRequirementStatus(int requirementId, string Status)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name)
                && SecurityProvider.userAuthFromRequirement(requirementId, User.Identity.Name))
            {
                var rS = db.Requirements.Where(x => x.Id == requirementId).Single();
                rS.Status = Status;
                //check to make sure personel is right
                db.Requirements.Attach(rS);
                db.Entry(rS).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else return RedirectToAction("NotFound", "Error");

            
        }

    }
}
