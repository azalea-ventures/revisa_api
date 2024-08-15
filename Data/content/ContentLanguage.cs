using System;
using System.Collections.Generic;

namespace revisa_api.Data.content;

public partial class ContentLanguage
{
    public int Id { get; set; }

    public string? Language { get; set; }

    public string? Abbreviation { get; set; }

    public virtual ICollection<ContentDetail> ContentDetails { get; set; } = new List<ContentDetail>();
}
