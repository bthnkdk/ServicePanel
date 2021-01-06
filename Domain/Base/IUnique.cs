using System;

namespace Domain
{
    public interface IUnique
    {
        Guid RowId { get; set; }
    }
}