﻿using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult AddUser(String tenantId, string username, string type)
        {

            var person = new Personnel
            {
                Id = username,
                Name = username,
                TenantId = Convert.ToInt32(tenantId),
                Type = "User"
            };

            db.Personnels.Add(person);
            db.SaveChanges();

            return RedirectToAction("Index", new {  });
        }
    }
}
