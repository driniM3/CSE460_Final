using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagement.DataProviders
{
    public class SecurityProvider
    {
        
        public static bool tenantAuth(string tenant, string user)
        {
            var db = new Models.PMEntities();
            return (from p in db.Personnels
                    where p.Id == user && p.Tenant.Name.ToLower() == tenant.ToLower()
                    select p).Any();
        }

        public static bool tenantAuthFromProject(int projectId, string user)
        {
            var db = new Models.PMEntities();
            return (from p in db.Personnels
                    where p.Id == user && p.Tenant.Projects.Any(x=> x.Id == projectId)
                    select p).Any();
        }

        public static bool tenantAuthFromRequirement(int requirementId, string user)
        {
            var db = new Models.PMEntities();
            return (from p in db.Personnels
                    where p.Id == user && p.Tenant.Projects.Any(x => x.Requirements.Any(c=> c.Id == requirementId))
                    select p).Any();
        }

        public static bool managerAuthFromProject(int projectId, string user)
        {
            var db = new Models.PMEntities();
            return (from p in db.Personnels
                    where p.Id == user && p.Projects.Any(x => x.Id == projectId)
                    select p).Any();
        }

        public static bool managerAuthFromRequirement(int requirementId, string user)
        {
            var db = new Models.PMEntities();
            return (from p in db.Personnels
                    where p.Id == user && p.Projects.Any(x => x.Requirements.Any(c=> c.Id == requirementId))
                    select p).Any();
        }

        public static bool managerAuthFromUser(string u, string user)
        {
            var db = new Models.PMEntities();
            return (from p in db.Personnels
                    where p.Id == user 
                    from q in db.Personnels
                    where q.Id == u && q.Type == "User" && q.TenantId == p.TenantId
                    select q).Any();
        }

        public static bool userAuthFromProject(int projectId, string user)
        {
            var db = new Models.PMEntities();
            return (from p in db.Personnels
                    where p.Id == user && p.ProjectPersonnels.Any(x => x.ProjectId == projectId)
                    select p).Any();
        }

        public static bool userAuthFromRequirement(int requirementId, string user)
        {
            var db = new Models.PMEntities();
            return (from p in db.Personnels
                    where p.Id == user && p.ProjectPersonnels.Any(x => x.Project.Requirements.Any(c => c.Id == requirementId))
                    select p).Any();
        }
    }
}