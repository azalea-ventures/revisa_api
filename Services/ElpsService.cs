using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using revisa_api.Data.elps;

public interface IElpsService
{
    StrategyObjective GetStrategyObjective(int lessonOrder);
    void SetElpsSupports(ElpsSupportsRequest request);
}

public class ElpsService : IElpsService
{
    private readonly ElpsContext _dbContext;

    public ElpsService(ElpsContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SetElpsSupports(ElpsSupportsRequest request)
    {
        using var dbContext = _dbContext;
        string[] objectives = request
            .DomainObjectiveData.Where(dod => !dod.Domain.Equals(""))
            .Select(dod => dod.DomainObjectiveStatement)
            .ToArray();

        List<DomainObjective> domainObjectives = dbContext
            .DomainObjectives.Include(dobj => dobj.Domain)
            .Where(d => objectives.Contains(d.Objective.ToLower()))
            .ToList();

        domainObjectives.ForEach(domainObjective =>
        {
            DomainObjectiveMedia? media = request
                .DomainObjectiveData.Where(data =>
                    domainObjective.Objective.ToLower().Equals(data.DomainObjectiveStatement)
                )
                .FirstOrDefault();

            if (media != null)
            {
                string stringifiedData = JsonSerializer.Serialize(media.RichText);
                domainObjective.ObjectiveRichText = stringifiedData;
            }
        });

        string[] strategies = request
            .StrategyData.Select(strategy => strategy.StrategyStatement)
            .ToArray();

        List<LearningStrategiesMod> learningStrategiesMods = dbContext
            .LearningStrategiesMods.Include(lsm => lsm.LearningStrategy)
            .Where(lsm => strategies.Contains(lsm.Strategy.ToLower()))
            .ToList();

        learningStrategiesMods.ForEach(lsm =>
        {
            StrategyMedia? media = request
                .StrategyData.Where(data => lsm.Strategy.ToLower().Equals(data.StrategyStatement))
                .FirstOrDefault();

            if (media != null)
            {
                string stringifiedData = JsonSerializer.Serialize(media.RichText);
                lsm.StrategyRichText = stringifiedData;
            }
        });

        dbContext.SaveChanges();

    }

    public StrategyObjective GetStrategyObjective(int lessonOrder)
    {
        using var dbContext = _dbContext;
        return dbContext.StrategyObjectives.Where(x => x.Id == lessonOrder).FirstOrDefault();
    }
}
