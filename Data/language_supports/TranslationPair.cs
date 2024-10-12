
namespace revisa_api.Data.language_supports;

public partial class TranslationPair
{
    public int Id { get; set; }

    public int? LanguageOriginId { get; set; }

    public string LanguageOriginText { get; set; } = null!;

    public int? LanguageTargetId { get; set; }

    public string LanguageTargetText { get; set; } = null!;

    public string? LanguageTargetMeaning { get; set; }

    public virtual Language? LanguageOrigin { get; set; }

    public virtual Language? LanguageTarget { get; set; }
}
