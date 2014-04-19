using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagement.DataProviders
{
    public static class TenantProvider
    {
        public static TenantModel getTenantStyle(string tenant)
        {
            var db = new PMEntities();
            if (db.Tenants.Any(x => x.Name.ToLower() == tenant.ToLower()))
            {
                var t = db.Tenants.Where(x => x.Name.ToLower() == tenant.ToLower()).Single();
                return new TenantModel { 
                    id = t.Id,
                    Name = t.Name,
                    Style = t.Style,
                    Logo = t.Logo
                };
            }
            else
            {
                return new TenantModel
                {
                    id = 0,
                    Name = "Project Management",
                    Style = "bootstrap.min.css"
                };
            }
        }
    }
}