using System;

namespace Domain
{
    public class File : Entity, IUnique
    {
        public Guid RowId { get; set; }
        public Guid StoreId { get; set; }
        public int FileTypeId { get; set; }
        public string FileName { get; set; }
        public long ContentLength { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public virtual AppUser CreatedAppUser { get; set; }
        public virtual FileType FileType { get; set; }
    }
    public class FileType : Entity
    {
        public string Name { get; set; }

    }
}
