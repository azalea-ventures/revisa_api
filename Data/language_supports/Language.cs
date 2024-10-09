
namespace revisa_api.Data.language_supports;

public partial class Language
{
    public int Id { get; set; }

    public string? LanguageShort { get; set; }

    public string LanguageName { get; set; } = null!;

    public virtual ICollection<TranslationPair> TranslationPairLanguageOrigins { get; set; } = new List<TranslationPair>();

    public virtual ICollection<TranslationPair> TranslationPairLanguageTargets { get; set; } = new List<TranslationPair>();
}
