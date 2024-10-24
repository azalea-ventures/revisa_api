using revisa_api.Data.language_supports;

public class PvrResponse
{
    public PvrResponse() { }

    public PvrResponse(TranslationPvrRule rule, string grade, string subject)
    {
        TeacherTitle = rule.TeacherTitleNavigation.Language;
        TeacherContent = rule.TeacherContentNavigation.Language;
        StudentTitle = rule.StudentTitleNavigation.Language;
        PreviewBody = rule.PreviewBodyNavigation.Language;
        PreviewNotes = rule.PreviewNotesNavigation.Language;
        ViewBody = rule.ViewBodyNavigation.Language;
        ViewNotes = rule.ViewNotesNavigation.Language;
        ReviewBody = rule.ReviewBodyNavigation.Language;
        ReviewNotes = rule.ReviewNotesNavigation.Language;
        Grade = grade;
        Subject = subject;
    }

    public string? Grade { get; set; }
    public string? Subject { get; set; }
    public string? TeacherTitle { get; set; }
    public string? TeacherContent { get; set; }
    public string? StudentTitle { get; set; }
    public string? PreviewBody { get; set; }
    public string? PreviewNotes { get; set; }
    public string? ViewBody { get; set; }
    public string? ViewNotes { get; set; }
    public string? ReviewBody { get; set; }
    public string? ReviewNotes { get; set; }
}
