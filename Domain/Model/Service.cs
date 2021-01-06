using System;
using System.Collections.Generic;

namespace Domain
{
    public class Service : Entity, IUnique
    {
        public override string AuthorityCode => "SVC";

        public Service()
        {
            ServicePrinters = new List<ServicePrinter>();
        }
        public Guid RowId { get; set; }
        public int LocationId { get; set; }
        public int ServiceCategoryId { get; set; }
        public double Price { get; set; }
        public int? PriceCurrency { get; set; }
        public string Description { get; set; }
        public string Process { get; set; }
        public string ResponsibleName { get; set; }
        public string ResponsiblePhone { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdateUserId { get; set; }
        public virtual Location Location { get; set; }
        public virtual AppUser CreatedUser { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
        public virtual List<ServicePrinter> ServicePrinters { get; set; }
        public virtual List<ServicePerson> ServicePersons { get; set; }
        public virtual List<ServiceVehicle> ServiceVehicles { get; set; }
        public virtual List<ServiceStock> ServiceStocks { get; set; }
    }
    public class ServiceCategory : DelEntity
    {
        public override string AuthorityCode => "DEF";

        public ServiceCategory()
        {
            Services = new HashSet<Service>();
        }
        public string Name { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
    public class ServicePerson : Entity
    {
        public override string AuthorityCode => "SVC";

        public int AppUserId { get; set; }
        public int ServiceId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Service Service { get; set; }
    }
    public class ServicePrinter : Entity
    {
        public override string AuthorityCode => "SVC";

        public int ServiceId { get; set; }
        public int PrinterId { get; set; }
        public int PrinterServiceTypeId { get; set; }
        public string Description { get; set; }
        public string Process { get; set; }
        public int? Mono { get; set; }
        public int? Color { get; set; }
        public bool IsMaintenanceOk { get; set; }
        public int Status { get; set; }
        public int? CounterId { get; set; }

        public virtual Service Service { get; set; }
        public virtual Printer Printer { get; set; }
        public virtual PrinterServiceType PrinterServiceType { get; set; }
        public virtual Counter Counter { get; set; }
    }
    public class ServiceStock : Entity
    {
        public override string AuthorityCode => "SVC";
        public int ServiceId { get; set; }
        public int StockId { get; set; }
        public int Count { get; set; }
        public int Status { get; set; }
        public bool IsDelivered { get; set; }
        public int? StockMovementId { get; set; }
        public int? PrinterId { get; set; }

        public virtual Service Service { get; set; }
        public virtual Stock Stock { get; set; }
        public virtual StockMovement StockMovement { get; set; }
    }
    public class ServiceVehicle : Entity
    {
        public override string AuthorityCode => "SVC";

        public int VehicleId { get; set; }
        public int ServiceId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual Service Service { get; set; }
    }
}
