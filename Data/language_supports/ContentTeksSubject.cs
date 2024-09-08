using System;
using System.Collections.Generic;
using revisa_api.Data.content;
using revisa_api.Data.teks;

namespace revisa_api.Data.language_supports;

public partial class ContentTeksSubject
{
    public int Id { get; set; }

    public int? ContentSubjectId { get; set; }

    public Guid? TeksSubjectId { get; set; }

    public virtual Subject? ContentSubject { get; set; }

    public virtual TeksSubject? TeksSubject { get; set; }
}
