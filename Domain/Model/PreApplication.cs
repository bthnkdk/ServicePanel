using System;
using System.Collections.Generic;

namespace Domain
{
    public class PreApplication : DelEntity
    {
        public PreApplication()
        {
            PreApplicationLogs = new HashSet<PreApplicationLog>();
        }
        public override string AuthorityCode => "CRM";
        public int PreApplicationTypeId { get; set; }
        public int PreApplicationStatusId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAuthorized { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Description { get; set; }

        public virtual ICollection<PreApplicationLog> PreApplicationLogs { get; set; }
        public virtual PreApplicationType PreApplicationType { get; set; }
        public virtual PreApplicationStatus PreApplicationStatus { get; set; }
        public virtual AppUser CreatedUser { get; set; }
        public virtual AppUser UpdatedUser { get; set; }

    }
    public class PreApplicationLog : Entity
    {
        public override string AuthorityCode => "CRM";

        public int PreApplicationId { get; set; }
        public int PreApplicationLogStatusId { get; set; }
        public string Description { get; set; }
        public DateTime RemindDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual PreApplication PreApplication { get; set; }
        public virtual AppUser CreatedUser { get; set; }
        public virtual AppUser UpdatedUser { get; set; }
    }
    public class PreApplicationStatus : Entity
    {
        public PreApplicationStatus()
        {
            PreApplicationLogs = new HashSet<PreApplicationLog>();
        }

        public override string AuthorityCode => "DEF";
        public string Name { get; set; }
        public virtual ICollection<PreApplicationLog> PreApplicationLogs { get; set; }
    }
    public class PreApplicationType : Entity
    {
        public PreApplicationType()
        {
            PreApplications = new HashSet<PreApplication>();
        }

        public override string AuthorityCode => "DEF";
        public string Name { get; set; }
        public virtual ICollection<PreApplication> PreApplications { get; set; }
    }
}
