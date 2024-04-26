using System;
using System.Collections.Generic;

namespace revisa_api.Data.elps;

public partial class DomainLvlAttr
{
    public int Id { get; set; }

    public int? DomainLevelId { get; set; }

    public int? GradeId { get; set; }

    public string? Attr { get; set; }

    public int? AttrTypeId { get; set; }

    public virtual AttrType? AttrType { get; set; }

    public virtual DomainLevel? DomainLevel { get; set; }

    public virtual ICollection<DomainLvlAttrItem> DomainLvlAttrItems { get; set; } = new List<DomainLvlAttrItem>();

    public virtual Grade? Grade { get; set; }
}
