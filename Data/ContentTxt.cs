using System;
using System.Collections.Generic;

namespace revisa_api.Data;

public partial class ContentTxt
{
    public int Id { get; set; }

    public int ContentVersionId { get; set; }

    public int ContentTypeId { get; set; }

    public string? Content { get; set; }

    public virtual ContentType ContentType { get; set; } = null!;

    public virtual ContentVersion ContentVersion { get; set; } = null!;
}
