using System;
using System.Collections.Generic;

namespace revisa_api.Data;

public partial class AttrType
{
    public int Id { get; set; }

    public string? Typ { get; set; }

    public virtual ICollection<DomainLvlAttr> DomainLvlAttrs { get; set; } = new List<DomainLvlAttr>();
}
