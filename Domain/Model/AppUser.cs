using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Helpers;

namespace Domain
{
    public class AppUser : DelEntity, IUnique
    {
        public AppUser()
        {
            AppUserAuthorities = new HashSet<AppUserAuthority>();
            AppUserPermissions = new HashSet<AppUserPermission>();
            Vehicles = new HashSet<Vehicle>();
        }

        public override string AuthorityCode => "SYS";

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string SubTitle { get; set; }
        public Guid RowId { get; set; }
        public string Password { get; set; }
        public string Pin { get; set; }
        public string TokenKey { get; set; }
        public bool IsLock { get; set; }
        public bool IsAdmin { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
        public int TitleId { get; set; }
        public int DepartmentId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? EndDate { get; set; }
        public string FullName { get { return Firstname + " " + Lastname; } }

        public virtual Title Title { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<AppUserAuthority> AppUserAuthorities { get; set; }
        public virtual ICollection<AppUserPermission> AppUserPermissions { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<StockUserInventory> StockUserInventories { get; set; }

        public bool VerifyHashedPassword(string password)
        {
            return Crypto.VerifyHashedPassword(Password, password);
        }

        public void HashPassword(string password)
        {
            Password = Crypto.HashPassword(password);
        }

    }
    public class Title : DelEntity
    {
        public Title()
        {
            AppUsers = new HashSet<AppUser>();
        }

        public override string AuthorityCode => "SYS";
        public string Name { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }
    }

    public class AppUserAuthority
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AppUserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AuthCodeId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Access { get; set; }
        public virtual AuthCode AuthCode { get; set; }
        public virtual AppUser AppUser { get; set; }
    }

    public class AppUserLog : Entity
    {
        public override string AuthorityCode => "SYS";

        public string Username { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public string Os { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }

    public class AppUserPermission : Entity
    {
        public override string AuthorityCode => "PLN";
        public int AppUserId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string CreatedUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime CreatedDate { get; set; }
        public int Duration { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
