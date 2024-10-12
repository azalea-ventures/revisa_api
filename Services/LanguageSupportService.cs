using Microsoft.EntityFrameworkCore;
using revisa_api.Data.language_supports;

public interface ILanguageSupportService
{
    ElpsSupportResponse GetElpsSupports(string delivery_date);
    LessonSchedule GetLessonSchedule(DateOnly delivery_date);
}

public class LanguageSupportService : ILanguageSupportService
{
    private readonly IDbContextFactory<LanguageSupportContext> _languageSupportContextFactory;
    private readonly ITeksService _teksService;
    private readonly IElpsService _elpsService;

    public LanguageSupportService(
        IDbContextFactory<LanguageSupportContext> languageSupportContextFactory,
        ITeksService teksService,
        IElpsService elpsService
    )
    {
        _languageSupportContextFactory = languageSupportContextFactory;
        _teksService = teksService;
        _elpsService = elpsService;
    }

    public ElpsSupportResponse GetElpsSupports(string delivery_date)
    {
        using var languageSupportContext = _languageSupportContextFactory.CreateDbContext();

        var lesson_schedule = languageSupportContext.LessonSchedules.FirstOrDefault(s =>
            s.DeliveryDate == DateOnly.Parse(delivery_date)
        );

        if (lesson_schedule == null)
        {
            return new();
        }


        ElpsSupportResponse response = null;
        return response;
    }

 
    public LessonSchedule GetLessonSchedule(DateOnly delivery_date)
    {
        using var languageSupportContext = _languageSupportContextFactory.CreateDbContext();
        var schedules = languageSupportContext.LessonSchedules.Select(s => s).ToList();
        return languageSupportContext.LessonSchedules.FirstOrDefault(s =>
            s.DeliveryDate == delivery_date
        );
    }

}
