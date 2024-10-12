using revisa_api.Data.language_supports;

namespace revisa_api.Data.elps;

public partial class StrategyObjective
{
    public int Id { get; set; }

    public int StrategyModId { get; set; }

    public int DomainObjectiveId { get; set; }

    public virtual DomainObjective DomainObjective { get; set; } = null!;

    public virtual ICollection<SupportPackage> SupportPackages { get; set; } =
        new List<SupportPackage>();

    public virtual LearningStrategiesMod StrategyMod { get; set; } = null!;

}
