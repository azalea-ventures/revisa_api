using System;
using System.Collections.Generic;
using revisa_api.Data.language_supports;

namespace revisa_api.Data.content;

public partial class ContentLanguage
{
    public int Id { get; set; }

    public string? Language { get; set; }

    public string? Abbreviation { get; set; }
        public virtual ICollection<SupportPackage> SupportPackages { get; set; } = new List<SupportPackage>();


    public virtual ICollection<ContentDetail> ContentDetails { get; set; } = new List<ContentDetail>();

    public virtual ICollection<ContentTranslation> ContentTranslationContentLanguages { get; set; } = new List<ContentTranslation>();

    public virtual ICollection<ContentTranslation> ContentTranslationTargetLanguages { get; set; } = new List<ContentTranslation>();
}
