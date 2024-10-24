using System;
using System.Collections.Generic;
using revisa_api.Data.content;

namespace revisa_api.Data.language_supports;

public partial class TranslationPvrRule
{
    public int ContentTranslationId { get; set; }

    public int TeacherTitle { get; set; }

    public int StudentTitle { get; set; }

    public int TeacherContent { get; set; }

    public int PreviewBody { get; set; }

    public int PreviewNotes { get; set; }

    public int ViewBody { get; set; }

    public int ViewNotes { get; set; }

    public int ReviewBody { get; set; }

    public int ReviewNotes { get; set; }

    public virtual ContentTranslation ContentTranslation { get; set; } = null!;

    public virtual ContentLanguage PreviewBodyNavigation { get; set; } = null!;

    public virtual ContentLanguage PreviewNotesNavigation { get; set; } = null!;

    public virtual ContentLanguage ReviewBodyNavigation { get; set; } = null!;

    public virtual ContentLanguage ReviewNotesNavigation { get; set; } = null!;

    public virtual ContentLanguage StudentTitleNavigation { get; set; } = null!;

    public virtual ContentLanguage TeacherContentNavigation { get; set; } = null!;

    public virtual ContentLanguage TeacherTitleNavigation { get; set; } = null!;

    public virtual ContentLanguage ViewBodyNavigation { get; set; } = null!;

    public virtual ContentLanguage ViewNotesNavigation { get; set; } = null!;
}
