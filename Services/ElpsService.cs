using revisa_api.Data.elps;

public class ElpsService : IElpsService {
    private readonly ElpsContext _dbContext;

    public ElpsService(ElpsContext dbContext) {

        _dbContext = dbContext;

    }

    public StrategiesObjective GetStrategyObjective(int lessonOrder){
        using var dbContext = _dbContext;
        return dbContext.StrategiesObjectives.Where(x => x.Id == lessonOrder).FirstOrDefault();
    }
}