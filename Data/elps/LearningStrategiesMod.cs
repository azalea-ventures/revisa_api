using System;
using System.Collections.Generic;

namespace revisa_api.Data.elps;

public partial class LearningStrategiesMod
{
    public int Id { get; set; }

    public int LearningStrategyId { get; set; }

    public string? Strategy { get; set; }

    public string? ImageFileId { get; set; }

    public virtual LearningStrategy LearningStrategy { get; set; } = null!;

    public virtual ICollection<StrategyObjective> StrategiesObjectives { get; set; } = new List<StrategyObjective>();
}
