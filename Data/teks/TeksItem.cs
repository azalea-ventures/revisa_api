using System;
using System.Collections.Generic;

namespace revisa_api.Data.teks;

public partial class TeksItem
{
    public Guid Id { get; set; }

    public Guid? ParentId { get; set; }

    public int ListEnumeration { get; set; }

    public Guid? ItemTypeId { get; set; }

    public string? HumanCodingScheme { get; set; }

    public string? FullStatement { get; set; }

    public string? Language { get; set; }

    public DateTime? LastChangeTea { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual ICollection<TeksItem> InverseParent { get; set; } = new List<TeksItem>();

    public virtual TeksItemType? ItemType { get; set; }

    public virtual TeksItem? Parent { get; set; }
}
