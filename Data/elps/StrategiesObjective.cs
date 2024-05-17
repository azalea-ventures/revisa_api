using System;
using System.Collections.Generic;
using revisa_api.Data.language_supports;

namespace revisa_api.Data.elps;

public partial class StrategiesObjective
{
    public int Id { get; set; }

    public int StrategyModId { get; set; }

    public int DomainObjectiveId { get; set; }

    public virtual DomainObjective DomainObjective { get; set; } = null!;

    public virtual ICollection<Iclo> Iclos { get; set; } = new List<Iclo>();

    public virtual LearningStrategiesMod StrategyMod { get; set; } = null!;
}
