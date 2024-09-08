using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using revisa_api.Data.language_supports;

namespace revisa_api.Data.content;

public partial class ContentLanguage
{
    public int Id { get; set; }

    public string? Language { get; set; }

    public string? Abbreviation { get; set; }

    public virtual ICollection<ContentDetail> ContentDetails { get; set; } = new List<ContentDetail>();
    [NotMapped]
    public virtual ICollection<ContentTranslation> ContentTranslationSourceLanguages { get; set; } = new List<ContentTranslation>();
    [NotMapped]
    public virtual ICollection<ContentTranslation> ContentTranslationTargetLanguages { get; set; } = new List<ContentTranslation>();
    public virtual ICollection<SupportPackage> SupportPackages { get; set; }
}
