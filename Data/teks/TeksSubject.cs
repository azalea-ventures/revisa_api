using System;
using System.Collections.Generic;
using revisa_api.Data.language_supports;

namespace revisa_api.Data.teks;

public partial class TeksSubject
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? TacChapter { get; set; }

    public virtual ICollection<Tek> Teks { get; set; } = new List<Tek>();
    public virtual ICollection<ContentTeksSubject> ContentTeksSubjects { get; set; } =
        new List<ContentTeksSubject>();
}
