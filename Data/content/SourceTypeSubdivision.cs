using System;
using System.Collections.Generic;

namespace revisa_api.Data.content;

public partial class SourceTypeSubdivision
{
    public int SourceTypeId { get; set; }

    public string? SubdivName { get; set; }

    public byte SubdivLevel { get; set; }

    public virtual SourceType SourceType { get; set; } = null!;
}
