//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Project
    {
        public Project()
        {
            this.ProjectPersonnels = new HashSet<ProjectPersonnel>();
            this.Requirements = new HashSet<Requirement>();
        }
    
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> ExpectedEndDate { get; set; }
        public string Status { get; set; }
        public string Manager { get; set; }
    
        public virtual Personnel Personnel { get; set; }
        public virtual ICollection<ProjectPersonnel> ProjectPersonnels { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<Requirement> Requirements { get; set; }
    }
}
