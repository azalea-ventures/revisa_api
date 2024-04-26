using System;
using System.Collections.Generic;

namespace revisa_api.Data.elps;

public partial class DomainLevel
{
    public int Id { get; set; }

    public int? DomainId { get; set; }

    public int? LevelId { get; set; }

    public string? Details { get; set; }

    public virtual Domain? Domain { get; set; }

    public virtual ICollection<DomainLvlAttr> DomainLvlAttrs { get; set; } = new List<DomainLvlAttr>();

    public virtual Level? Level { get; set; }
}
