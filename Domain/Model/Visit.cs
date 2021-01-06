using System;
using System.Collections.Generic;

namespace Domain
{
    public class Visit : DelEntity
    {
        public Visit()
        {
            VisitLogs = new HashSet<VisitLog>();
        }

        public override string AuthorityCode => "CRM";
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public int VisitCategoryId { get; set; }
        public int VisitTypeId { get; set; }
        public int AppUserId { get; set; }
        public DateTime VisitDate { get; set; }
        public int Status { get; set; }
        public string Subject { get; set; }
        public int CustomerId { get; set; }

        public virtual VisitType VisitType { get; set; }
        public virtual VisitCategory VisitCategory { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual AppUser CreatedUser { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<VisitLog> VisitLogs { get; set; }
    }
    public class VisitCategory : DelEntity
    {
        public override string AuthorityCode => "DEF";

        public VisitCategory()
        {
            Visits = new HashSet<Visit>();
        }
        public string Name { get; set; }
        public virtual ICollection<Visit> Visits { get; set; }
    }
    public class VisitLog : Entity
    {
        public override string AuthorityCode => "CRM";

        public int Status { get; set; }
        public int VisitId { get; set; }
        public int AppUserId { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public DateTime Date { get; set; }

        public virtual Visit Visit { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
    public class VisitType : DelEntity
    {
        public override string AuthorityCode => "DEF";

        public VisitType()
        {
            Visits = new HashSet<Visit>();
        }
        public string Name { get; set; }
        public virtual ICollection<Visit> Visits { get; set; }
    }

}