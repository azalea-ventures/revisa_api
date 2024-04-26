using System;
using System.Collections.Generic;

namespace revisa_api.Data.teks;

public partial class TeksSubject
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Tek> Teks { get; set; } = new List<Tek>();
}
