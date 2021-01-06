using System;
using System.Collections.Generic;

namespace Domain
{
    public class Customer : DelEntity, IUnique
    {
        public Customer()
        {
            Locations = new HashSet<Location>();
        }

        public override string AuthorityCode => "CRM";

        public Guid RowId { get; set; }
        public string Name { get; set; }
        public string CustomerCode { get; set; }
        public string WebSite { get; set; }
        public int CustomerTypeId { get; set; }
        public string VerDar { get; set; }
        public string VerNo { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual CustomerType CustomerType { get; set; }
        public virtual AppUser CreatedUser { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
    public class CustomerType : DelEntity
    {
        public override string AuthorityCode => "DEF";

        public CustomerType()
        {
            Customers = new HashSet<Customer>();
        }
        public string Name { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
    public class Location : DelEntity, IDelete, IUnique
    {
        public Location()
        {
            PrinterMovements = new HashSet<PrinterMovement>();
        }

        public override string AuthorityCode => "CRM";

        public Guid RowId { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public int CityId { get; set; }
        public int TownId { get; set; }
        public string Address { get; set; }
        public string ResponsibleName { get; set; }
        public string ResponsibleMail { get; set; }
        public string ResponsiblePhone { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DefaultAppUserId { get; set; }

        public virtual AppUser DefaultAppUser { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual AppUser CreatedUser { get; set; }
        public virtual City City { get; set; }
        public virtual Town Town { get; set; }
        public virtual ICollection<PrinterMovement> PrinterMovements { get; set; }
    }
}
