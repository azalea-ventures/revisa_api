﻿using System;
using System.Collections.Generic;

namespace revisa_api.Data.elps;

public partial class DomainObjective
{
    public int Id { get; set; }

    public int? DomainId { get; set; }

    public string? Label { get; set; }

    public string? Objective { get; set; }

    public string? ObjectiveRichText { get; set; }

    public virtual Domain? Domain { get; set; }

    public virtual ICollection<StrategiesObjective> StrategiesObjectives { get; set; } = new List<StrategiesObjective>();
}
