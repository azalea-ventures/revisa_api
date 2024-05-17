using System;
using System.Collections.Generic;

namespace revisa_api.Data.language_supports;

public partial class LessonSchedule
{
    public int Id { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int LessonOrder { get; set; }
}
