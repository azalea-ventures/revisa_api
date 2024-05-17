using System;
using System.Collections.Generic;

namespace revisa_api.Data.elps;

public partial class Level
{
    public int Id { get; set; }

    public string? Lvl { get; set; }

    public virtual ICollection<DomainLevel> DomainLevels { get; set; } = new List<DomainLevel>();
}
