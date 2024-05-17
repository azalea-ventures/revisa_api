using System;
using System.Collections.Generic;

namespace revisa_api.Data.teks;

public partial class Tek
{
    public Guid Id { get; set; }

    public Guid? SubjectId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? AdoptionStatus { get; set; }

    public string? EffectiveYear { get; set; }

    public string? Notes { get; set; }

    public string? OfficialSourceUrl { get; set; }

    public string? Language { get; set; }

    public virtual TeksSubject? Subject { get; set; }
}
