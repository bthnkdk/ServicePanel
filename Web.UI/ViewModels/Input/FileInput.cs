using System;
using System.ComponentModel.DataAnnotations;

namespace Web.UI.ViewModels
{
    public class FileInput : BaseInput
    {
        public Guid RowId { get; set; }
        public Guid StoreId { get; set; }
        [Display(Name = "Açıklama")]
        [MaxLength(500, ErrorMessage = "Max {1}")]
        [UIHint("Textarea")]
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Dosya Adı")]
        [MaxLength(150, ErrorMessage = "Max {1}")]
        public string Name { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public Int64 Length { get; set; }
    }
}