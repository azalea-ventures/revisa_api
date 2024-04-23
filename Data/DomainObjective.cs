using System;
using System.Collections.Generic;

namespace revisa_api.Data;

public partial class DomainObjective
{
    public int Id { get; set; }

    public int? DomainId { get; set; }

    public string? Label { get; set; }

    public string? Objective { get; set; }

    public virtual Domain? Domain { get; set; }
}
