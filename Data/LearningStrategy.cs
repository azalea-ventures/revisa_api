using System;
using System.Collections.Generic;

namespace revisa_api.Data;

public partial class LearningStrategy
{
    public int Id { get; set; }

    public string? Label { get; set; }

    public string? Objective { get; set; }
}
