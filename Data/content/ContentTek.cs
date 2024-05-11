using System;
using System.Collections.Generic;
using revisa_api.Data.teks;

namespace revisa_api.Data.content;

public partial class ContentTek
{
    public int ContentVersionId { get; set; }

    public Guid TekItemId { get; set; }

    public virtual ContentVersion ContentVersion { get; set; } = null!;

    public virtual TeksItem TekItem { get; set; } = null!;
}
