using revisa_api.Data.elps;
using revisa_api.Data.language_supports;
using revisa_api.Data.teks;

public interface ILanguageSupportService
{
    ElpsSupportResponse GetElpsSupports(string delivery_date);
    PostContentResponse GetElpsSupportsByIcloId(int icloId);
    LessonSchedule GetLessonSchedule(DateOnly delivery_date);
    Iclo GetIclo(
        List<TeksItem> teks,
        LessonSchedule lessonSchedule,
        StrategyObjective strategyObjective
    );

    void CreateIclo(
        List<string> teks,
        string grade,
        revisa_api.Data.content.Subject subject,
        DateOnly deliveryDate
    );
}
