using System;
using System.Collections.Generic;

namespace revisa_api.Data.language_supports;

public partial class ContentTeksSubject
{
    public int Id { get; set; }

    public int? ContentSubjectId { get; set; }

    public Guid? TeksSubjectId { get; set; }
}
