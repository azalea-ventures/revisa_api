using System;
using System.Collections.Generic;

namespace revisa_api.Data.language_supports;

public partial class Iclo
{
    public int Id { get; set; }

    public string Iclo1 { get; set; } = null!;

    public int StrategyObjectiveId { get; set; }

    public Guid TeksItemId { get; set; }

    public int? LessonScheduleId { get; set; }

    public virtual LessonSchedule? LessonSchedule { get; set; }
}
