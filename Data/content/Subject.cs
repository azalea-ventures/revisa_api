using System;
using System.Collections.Generic;
using revisa_api.Data.language_supports;

namespace revisa_api.Data.content;

public partial class Subject
{
    public int Id { get; set; }

    public string Subject1 { get; set; } = null!;

    public virtual ICollection<ContentDetail> ContentDetails { get; set; } = new List<ContentDetail>();

    public virtual ICollection<ContentTranslation> ContentTranslations { get; set; } = new List<ContentTranslation>();

    public virtual ICollection<SourceContent> SourceContents { get; set; } = new List<SourceContent>();

    public virtual ICollection<SupportPackage> SupportPackages { get; set; } = new List<SupportPackage>();
}
