using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectManagement.DataProviders;

namespace ProjectManagement.Controllers.Views
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/
        private PMEntities db = new PMEntities();

        public ActionResult Index(string tenant)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name))
            {
                Tenant te = db.Tenants.Where(x => x.Name == tenant).Single();
                var Manager = db.Personnels.Where(x => x.Id == User.Identity.Name).Single();
                ViewBag.Tenant = te;
                ViewBag.Users = te.Personnels.Where(x => x.Type == "User");
                return View(Manager);

            }
            else return RedirectToAction("NotFound", "Error");
        }

        public ActionResult Project(string tenant, int projectId)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name)
                && SecurityProvider.managerAuthFromProject(projectId, User.Identity.Name))
            {
                var project = db.Projects.Where(x => x.Id == projectId).Single();
                return View(project);
            }
            else return RedirectToAction("NotFound", "Error");

        }

        public ActionResult Requirement(string tenant, int requirementId)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name)
                && SecurityProvider.managerAuthFromRequirement(requirementId, User.Identity.Name))
            {
                var r = db.Requirements.Where(x => x.Id == requirementId).Single();
                return View(r);
            }
            else return RedirectToAction("NotFound", "Error");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProject(String TenantId, String Name, String StartDateDay, String StartDateMonth, String StartDateYear, String EndDateDay, String EndDateMonth, String EndDateYear, String status, String managerId, IEnumerable<String> users)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name))
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

                IEnumerable<Personnel> usersForProject = db.Personnels.Where(x => users.Contains(x.Name));

                foreach (Personnel person in usersForProject)
                {
                    var projectPersonnel = new ProjectPersonnel
                    {
                        Personnel = person,
                        Project = project
                    };

                    db.ProjectPersonnels.Add(projectPersonnel);

                    project.ProjectPersonnels.Add(projectPersonnel);
                }

                db.Entry(project).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { });
            }
            else return RedirectToAction("NotFound", "Error");


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployeeToProject(string employee, int projectId)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name)
                && SecurityProvider.managerAuthFromProject(projectId, User.Identity.Name)
                && SecurityProvider.managerAuthFromUser(employee, User.Identity.Name))
            {
                var project = db.Projects.Where(x => x.Id == projectId).Single();
                var personnel = db.Personnels.Where(x => x.Id == employee).Single();
                //check to make sure personel is right
                if (!(project.ProjectPersonnels.Any(x => x.Personnel.Id == employee) || project.Personnel.Id == employee || personnel.Type == "Manager"))
                {
                    var pP = new ProjectPersonnel
                    {
                        PersonnelId = employee,
                        ProjectId = projectId
                    };

                    db.ProjectPersonnels.Add(pP);
                    db.SaveChanges();
                }

                return RedirectToAction("Project", new { projectId = projectId });
            }
            else return RedirectToAction("NotFound", "Error");



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRequirementToProject(Requirement r)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name)
                && SecurityProvider.managerAuthFromProject(r.ProjectId, User.Identity.Name))
            {
                var project = db.Projects.Where(x => x.Id == r.ProjectId).Single();
                //check to make sure personel is right
                db.Requirements.Add(r);
                db.SaveChanges();

                return RedirectToAction("Project", new { projectId = project.Id });
            }
            else return RedirectToAction("NotFound", "Error");

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRequirementToProject(Requirement r)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name)
                && SecurityProvider.managerAuthFromProject(r.ProjectId, User.Identity.Name)
                && SecurityProvider.managerAuthFromRequirement(r.Id, User.Identity.Name))
            {
                var rS = db.Requirements.Single(x => x.Id == r.Id);
                rS.Name = r.Name;
                rS.Description = r.Description;
                rS.Type = r.Type;
                rS.Time = r.Time;
                rS.Assignee = r.Assignee;
                rS.Status = r.Status;
                //check to make sure personel is right
                db.Requirements.Attach(rS);
                db.Entry(rS).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Project", new { projectId = r.ProjectId });
            }
            else return RedirectToAction("NotFound", "Error");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProjectStatus(int projectId, string Status)
        {
            if (SecurityProvider.tenantAuth(RouteData.Values["tenant"].ToString(), User.Identity.Name)
                && SecurityProvider.managerAuthFromProject(projectId, User.Identity.Name))
            {
                var rS = db.Projects.Where(x => x.Id == projectId).Single();
                rS.Status = Status;
                //check to make sure personel is right
                db.Projects.Attach(rS);
                db.Entry(rS).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Project", new { projectId = rS.Id });
            }

            else return RedirectToAction("NotFound", "Error");
            
        }

    }
}
