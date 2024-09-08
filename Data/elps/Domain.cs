using System;
using System.Collections.Generic;

namespace revisa_api.Data.elps;

public partial class Domain
{
    public int Id { get; set; }

    public string? Domain1 { get; set; }

    public string? Label { get; set; }

    public string? ColorHexCode { get; set; }

    public virtual ICollection<DomainLevel> DomainLevels { get; set; } = new List<DomainLevel>();

    public virtual ICollection<DomainObjective> DomainObjectives { get; set; } = new List<DomainObjective>();
}
