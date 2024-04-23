using System;
using System.Collections.Generic;

namespace revisa_api.Data;

public partial class Grade1
{
    public int Id { get; set; }

    public string? Grade { get; set; }

    public virtual ICollection<DomainLvlAttr> DomainLvlAttrs { get; set; } = new List<DomainLvlAttr>();
}
