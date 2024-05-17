using System;
using System.Collections.Generic;

namespace revisa_api.Data.elps;

public partial class Grade
{
    public int Id { get; set; }

    public string? Grade1 { get; set; }

    public virtual ICollection<DomainLvlAttr> DomainLvlAttrs { get; set; } = new List<DomainLvlAttr>();
}
