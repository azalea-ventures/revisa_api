using revisa_api.Data.elps;
using revisa_api.Data.language_supports;
using revisa_api.Data.teks;

public interface ILanguageSupportService
{
    ElpsSupportResponse GetElpsSupports(string delivery_date);
    LessonSchedule GetLessonSchedule(DateOnly delivery_date);
    void AddIclo(
        List<TeksItem> teks,
        LessonSchedule lessonSchedule,
        StrategiesObjective strategyObjective
    );
}
