using System;
using System.Collections.Generic;

namespace revisa_api.Data;

public partial class ContentType
{
    public int Id { get; set; }

    public string ContentType1 { get; set; } = null!;

    public virtual ICollection<ContentTxt> ContentTxts { get; set; } = new List<ContentTxt>();
}
