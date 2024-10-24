using System;
using System.Collections.Generic;

namespace revisa_api.Data.language_supports;

public partial class LessonSchedule
{
    public int Id { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int LessonOrder { get; set; }

    public byte CycleNumber { get; set; }

    public byte WeekNumber { get; set; }

    public virtual ICollection<SupportPackage> SupportPackages { get; set; } = new List<SupportPackage>();
}
