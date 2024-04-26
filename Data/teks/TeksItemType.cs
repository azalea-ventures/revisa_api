using System;
using System.Collections.Generic;

namespace revisa_api.Data.teks;

public partial class TeksItemType
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<TeksItem> TeksItems { get; set; } = new List<TeksItem>();
}
