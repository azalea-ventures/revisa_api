using System;
using System.Collections.Generic;

namespace revisa_api.Data.elps;

public partial class LearningStrategy
{
    public int Id { get; set; }

    public string? Label { get; set; }

    public string? Strategy { get; set; }

    public virtual ICollection<LearningStrategiesMod> LearningStrategiesMods { get; set; } = new List<LearningStrategiesMod>();
}
