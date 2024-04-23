using System;
using System.Collections.Generic;

namespace revisa_api.Data;

public partial class Grade
{
    public int Id { get; set; }

    public string Grade1 { get; set; } = null!;

    public virtual ICollection<ContentDetail> ContentDetails { get; set; } = new List<ContentDetail>();
}
