using System;
using System.Collections.Generic;

namespace revisa_api.Data.content;

public partial class ClientFile
{
    public Guid Id { get; set; }

    public string? FileName { get; set; }

    public string? CurrentFolderId { get; set; }

    public Guid? OutbountFileId { get; set; }

    public Guid? OutboundFolderId { get; set; }

    public string? OutboundPath { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ContentDetail> ContentDetails { get; set; } = new List<ContentDetail>();
}
