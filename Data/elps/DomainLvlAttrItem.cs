using System;
using System.Collections.Generic;

namespace revisa_api.Data.elps;

public partial class DomainLvlAttrItem
{
    public int Id { get; set; }

    public int? DomainLvlAttrId { get; set; }

    public string? Item { get; set; }

    public virtual DomainLvlAttr? DomainLvlAttr { get; set; }
}
