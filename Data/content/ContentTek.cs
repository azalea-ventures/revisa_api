using System;
using System.Collections.Generic;

namespace revisa_api.Data.content;

public partial class ContentTek
{
    public int ContentVersionId { get; set; }

    public Guid TekItemId { get; set; }

    public virtual ContentVersion ContentVersion { get; set; } = null!;
}
