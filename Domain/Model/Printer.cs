using System;
using System.Collections.Generic;

namespace Domain
{
    public class Printer : DelEntity
    {
        public override string AuthorityCode => "PRT";

        public Printer()
        {
            PrinterMovements = new HashSet<PrinterMovement>();
            ServicePrinters = new HashSet<ServicePrinter>();
        }
        public Guid RowId { get; set; }
        public int PrinterModelId { get; set; }
        public int PrinterNumber { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public int? LastMaintenanceUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public string IPAddress { get; set; }
        public string Name { get; set; }
        public DateTime? PSRUpdateDate { get; set; }

        public virtual PrinterModel PrinterModel { get; set; }
        public virtual AppUser CreatedUser { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<PrinterMovement> PrinterMovements { get; set; }
        public virtual ICollection<ServicePrinter> ServicePrinters { get; set; }
    }
    public class PrinterBrand : Entity
    {
        public PrinterBrand()
        {
            Models = new HashSet<PrinterModel>();
        }
        public override string AuthorityCode => "DEF";
        public string Name { get; set; }
        public virtual ICollection<PrinterModel> Models { get; set; }
    }
    public class PrinterModel : Entity
    {
        public override string AuthorityCode => "DEF";
        public string Name { get; set; }
        public bool IsColor { get; set; }
        public int PrinterBrandId { get; set; }
        public virtual PrinterBrand PrinterBrand { get; set; }
    }
    public class PrinterMovement : Entity
    {
        public override string AuthorityCode => "PRT";

        public int PrinterId { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public DateTime? PSRUpdateDate { get; set; }
        public DateTime MoveDate { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
    }
    public class PrinterServiceType : DelEntity
    {
        public override string AuthorityCode => "DEF";

        public string Name { get; set; }
    }
    public class Counter : Entity
    {
        public override string AuthorityCode => "PRT";

        public int PrinterId { get; set; }
        public Guid RowId { get; set; }
        public int Mono { get; set; }
        public int? Color { get; set; }
        public int? Cyan { get; set; }
        public int? Magenta { get; set; }
        public int? Yellow { get; set; }
        public int? Black { get; set; }
        public DateTime Date { get; set; }

        public virtual Printer Printer { get; set; }
    }
    public class TonerChange : Entity
    {
        public override string AuthorityCode => "PRT";

        public int PrinterId { get; set; }
        public string Toner { get; set; }
        public int OldValue { get; set; }
        public int NewValue { get; set; }
        public DateTime Date { get; set; }
        public Guid CounterId { get; set; }
    }
}

