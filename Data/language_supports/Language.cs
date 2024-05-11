using System;
using System.Collections.Generic;

namespace revisa_api.Data.language_supports;

public partial class Language
{
    public int Id { get; set; }

    public string? LanguageShort { get; set; }

    public string LanguageName { get; set; } = null!;

    public virtual ICollection<Cognate> CognateLanguageOrigins { get; set; } = new List<Cognate>();

    public virtual ICollection<Cognate> CognateLanguageTargets { get; set; } = new List<Cognate>();
}
