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
using ProjectManagement.DataProviders;


namespace ProjectManagement.Controllers.Views
{
    [Authorize(Roles="Tenant Manager")]
    public class TenantManagementController : Controller
    {
        // GET: /TenantManagement/
        private PMEntities db = new PMEntities();
        
        public ActionResult Index()
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name))
            {
                var tenant = RouteData.Values["tenant"].ToString();
                var t = db.Tenants.Where(x => x.Name.ToLower() == tenant.ToLower()).Single();
                ViewBag.TenantRequirements = db.Requirements.Where(x => x.Personnel.TenantId == t.Id).ToList();
                return View(t);
            }
            else return  RedirectToAction("NotFound", "Error");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTenant(Tenant model)
        {

            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name))
            {
                var tenant = db.Tenants.Where(x => x.Id == model.Id).Single();
                tenant.Name = model.Name;
                tenant.Style = model.Style;
                db.Tenants.Attach(tenant);
                db.Entry(tenant).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { tenant = tenant.Name });
            }
            else return RedirectToAction("NotFound", "Error");
        }

            

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(String tenantId, string username, string password, string passwordConfirm, string type)
        {

            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name))
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
            else return RedirectToAction("NotFound", "Error");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLogo(string tenantId, string logoUrl)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name))
            {
                if (tenantId != null && logoUrl != null)
                {
                    int Id = Convert.ToInt32(tenantId);
                    var tenant = db.Tenants.Where(x => x.Id == Id).Single();

                    tenant.Logo = logoUrl;

                    db.Tenants.Attach(tenant);
                    db.Entry(tenant).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index", new { });
                }
                return RedirectToAction("Index", new { });
            }
            else return RedirectToAction("NotFound", "Error");

        }

        public ActionResult Project(string tenant, int projectId)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name))
            {
                var project = db.Projects.Where(x => x.Id == projectId).Single();
                return View(project);
            }
            else return RedirectToAction("NotFound", "Error");
        }

        public ActionResult Requirement(string tenant, int requirementId)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name))
            {
                var r = db.Requirements.Where(x => x.Id == requirementId).Single();
                return View(r);
            }
            else return RedirectToAction("NotFound", "Error");
        }
    }
}
