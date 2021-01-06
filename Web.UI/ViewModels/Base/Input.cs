using System.ComponentModel.DataAnnotations.Schema;

namespace Web.UI.ViewModels
{
    public class IInput
    {
        int Id { get; set; }
        bool IsNew { get; set; }
    }

    public class BaseInput : IInput
    {
        public virtual int Id { get; set; }

        [NotMapped]
        public virtual bool IsNew
        {
            get { if (Id == 0) return true; return false; }
        }
    }
}