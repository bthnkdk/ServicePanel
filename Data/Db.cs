using Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace Data
{
    public class Db : DbContext
    {
        public Db()
        {
            Database.SetInitializer<Db>(null);
        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AppUserAuthority> AppUserAuthoritis { get; set; }
        public virtual DbSet<AuthCode> AuthCodes { get; set; }
        public virtual DbSet<AppUserLog> AppUserLogs { get; set; }
        public virtual DbSet<AppUserPermission> AppUserPermissions { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Title> Titles { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }
        public virtual DbSet<VisitType> VisitTypes { get; set; }
        public virtual DbSet<VisitCategory> VisitCategories { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
        public virtual DbSet<PrinterBrand> PrinterBrands { get; set; }
        public virtual DbSet<PrinterModel> Models { get; set; }
        public virtual DbSet<PrinterServiceType> PrinterServiceTypes { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockMainCategory> StockMainCategories { get; set; }
        public virtual DbSet<StockCategory> StockCategories { get; set; }
        public virtual DbSet<PreApplication> PreApplications { get; set; }
        public virtual DbSet<PreApplicationLog> PreApplicationLogs { get; set; }
        public virtual DbSet<PreApplicationType> PreApplicationTypes { get; set; }
        public virtual DbSet<PreApplicationStatus> ApplicationLogStatuses { get; set; }
        public virtual DbSet<Counter> Counters { get; set; }
        public virtual DbSet<Printer> Printers { get; set; }
        public virtual DbSet<PrinterMovement> PrinterMovements { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServicePerson> ServicePersons { get; set; }
        public virtual DbSet<ServicePrinter> ServicePrinters { get; set; }
        public virtual DbSet<ServiceStock> ServiceStoks { get; set; }
        public virtual DbSet<ServiceVehicle> ServiceVehicles { get; set; }
        public virtual DbSet<StockMovement> StockMovements { get; set; }
        public virtual DbSet<VisitLog> VisitLogs { get; set; }
        public virtual DbSet<TonerChange> TonerChanges { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.AppUserPermissions)
                .WithRequired(e => e.AppUser)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.AppUserAuthorities)
                .WithRequired(e => e.AppUser);
            modelBuilder.Entity<AppUser>()
                .HasMany(s => s.StockUserInventories)
                .WithRequired(s => s.AppUser);
            modelBuilder.Entity<AppUser>()
                .HasMany(s => s.Vehicles)
                .WithRequired(s => s.AppUser);

            modelBuilder.Entity<AppUserAuthority>()
           .Property(e => e.Access)
           .IsFixedLength();

            modelBuilder.Entity<AuthCode>()
                .HasMany(s => s.Menus)
                .WithRequired(s => s.AuthCode);
            modelBuilder.Entity<Department>()
                .HasMany(s => s.AppUsers)
                .WithRequired(s => s.Department);
            modelBuilder.Entity<Menu>()
                .HasMany(e => e.Menu1)
                .WithOptional(e => e.Parent)
                .HasForeignKey(e => e.ParentId);
            modelBuilder.Entity<Title>()
               .HasMany(e => e.AppUsers)
               .WithRequired(e => e.Title)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<CustomerType>()
                .HasMany(s => s.Customers)
                .WithRequired(s => s.CustomerType);
            modelBuilder.Entity<VisitType>()
                .HasMany(s => s.Visits)
                .WithRequired(s => s.VisitType);
            modelBuilder.Entity<VisitCategory>()
                .HasMany(s => s.Visits)
                .WithRequired(s => s.VisitCategory);
            modelBuilder.Entity<PrinterBrand>()
               .HasMany(e => e.Models)
               .WithRequired(e => e.PrinterBrand)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<PreApplicationType>()
               .HasMany(s => s.PreApplications)
               .WithRequired(s => s.PreApplicationType)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Service>()
                .HasMany(s => s.ServicePrinters)
                .WithRequired(s => s.Service)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException valEx)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var validationErrors in valEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        sb.AppendFormat("{0} : {1}\r\n",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                throw new Exception(sb.ToString());
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException is System.Data.Entity.Core.UpdateException)
                {
                    var sqlException = ex.InnerException.InnerException as SqlException;
                    var number = sqlException.Number;
                    if (number == 2601)
                    {
                        string message = sqlException.Message;
                        if (message.Contains("FK_"))
                        {
                            throw new Exception(message);
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("Tekrarlanan Kayit",
                                Regex.Match(message, @"\(([^\)]+)\)"));
                            throw new Exception(sb.ToString());
                        }
                    }
                    else if (number == 2627)
                    {
                        throw new Exception("Tekrarlanan Kayıt !");
                    }
                    else if (number == 547)
                    {
                        throw new Exception("Bu kayıt farklı bir alana bağlı olduğu için silinemez !");
                    }
                    else
                    {
                        //547, 515, 206 etc...
                        throw new Exception(sqlException.Message);
                    }
                }
            }
            catch
            {
                throw;
            }
            return -1;
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var result = new DbEntityValidationResult(entityEntry, new List<DbValidationError>());

            if (result.ValidationErrors.Count > 0)
            {
                return result;
            }
            else
            {
                return base.ValidateEntity(entityEntry, items);
            }
        }
    }
}