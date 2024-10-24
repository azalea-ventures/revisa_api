﻿using System;
using System.Collections.Generic;
using revisa_api.Data.content;
using revisa_api.Data.elps;

namespace revisa_api.Data.language_supports;

public partial class SupportPackage
{
    public int Id { get; set; }

    public int ContentLanguageId { get; set; }

    public int GradeId { get; set; }

    public int SubjectId { get; set; }

    public int LessonScheduleId { get; set; }

    public int IsActive { get; set; }

    public int? ElpsStrategyObjectiveId { get; set; }

    public string? CrossLinguisticConnection { get; set; }

    public virtual ContentLanguage ContentLanguage { get; set; } = null!;

    public virtual StrategiesObjective? ElpsStrategyObjective { get; set; }

    public virtual content.Grade Grade { get; set; } = null!;

    public virtual LessonSchedule LessonSchedule { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
