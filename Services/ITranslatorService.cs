public interface ITranslatorService {
    Task<List<Content>> TranslateContent(List<List<Content> >content);
}