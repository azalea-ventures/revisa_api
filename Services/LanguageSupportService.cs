using Microsoft.EntityFrameworkCore;
using revisa_api.Data.elps;
using revisa_api.Data.language_supports;

public interface ILanguageSupportService
{
    ElpsSupportResponse GetElpsSupports(string delivery_date, string grade, string subject);
    LessonSchedule GetLessonSchedule(DateOnly delivery_date);
    PvrResponse GetPvrSupports(string grade, string subject, string language);
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

    public ElpsSupportResponse GetElpsSupports(string delivery_date, string grade, string subject)
    {
        ElpsSupportResponse response = new ElpsSupportResponse
        {
            PackageMeta = new SupportPackageMeta
            {
                deliveryDate = delivery_date,
                grade = grade,
                subject = subject
            }
        };

        using var languageSupportContext = _languageSupportContextFactory.CreateDbContext();

        SupportPackage? supportPackage = languageSupportContext
            .SupportPackages.Include(sp => sp.LessonSchedule)
            .Include(sp => sp.Grade)
            .Include(sp => sp.Subject)
            .Where(sp =>
                sp.LessonSchedule.DeliveryDate == DateOnly.Parse(delivery_date)
                && sp.Subject.Subject1 == subject
                && sp.Grade.Grade1 == grade
            )
            .FirstOrDefault();

        if (supportPackage == null || supportPackage.ElpsStrategyObjectiveId == null)
        {
            return response;
        }

        StrategiesObjective? strategyObjective = languageSupportContext
            .StrategiesObjectives.Include(sob => sob.StrategyMod)
            .ThenInclude(sm => sm.LearningStrategy)
            .Include(sob => sob.DomainObjective)
            .ThenInclude(dob => dob.Domain)
            .Where(sob => sob.Id == supportPackage.ElpsStrategyObjectiveId)
            .FirstOrDefault();

        response.ElpsStrategy = strategyObjective?.StrategyMod.Strategy;
        response.ElpsDomainName = strategyObjective?.DomainObjective.Domain?.Domain1;
        response.ElpsObjective = strategyObjective?.DomainObjective.Objective;
        response.ElpsStrategyLabel = strategyObjective?.StrategyMod.LearningStrategy.Label;
        response.ElpsStrategyFileId = strategyObjective?.StrategyMod.StrategyFileId;
        response.ElpsStrategyIconId = strategyObjective?.StrategyMod.ImageFileId;
        response.CrossLinguisticConnection = supportPackage.CrossLinguisticConnection;

        return response;
    }

    public PvrResponse GetPvrSupports(string grade, string subject, string language)
    {
        using var languageSupportContext = _languageSupportContextFactory.CreateDbContext();

        TranslationPvrRule? pvrRule = languageSupportContext
            .TranslationPvrRules.Include(pvr => pvr.PreviewBodyNavigation)
            .Include(pvr => pvr.PreviewNotesNavigation)
            .Include(pvr => pvr.ViewBodyNavigation)
            .Include(pvr => pvr.ViewNotesNavigation)
            .Include(pvr => pvr.StudentTitleNavigation)
            .Include(pvr => pvr.TeacherTitleNavigation)
            .Include(pvr => pvr.TeacherContentNavigation)
            .Include(pvr => pvr.ReviewBodyNavigation)
            .Include(pvr => pvr.ReviewNotesNavigation)
            .Include(pvr => pvr.ContentTranslation)
            .ThenInclude(ct => ct.ContentLanguage)
            .Where(pvr =>
                pvr.ContentTranslation.ContentGrade.Grade1 == grade
                && pvr.ContentTranslation.ContentSubject.Subject1 == subject
                && pvr.ContentTranslation.ContentLanguage.Abbreviation == language
            )
            .FirstOrDefault();
        
        return new(pvrRule, grade, subject);
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
