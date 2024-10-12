﻿using System;
using System.Collections.Generic;

namespace revisa_api.Data.content;

public partial class ContentStatus
{
    public int Id { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<ContentDetail> ContentDetails { get; set; } = new List<ContentDetail>();
}
