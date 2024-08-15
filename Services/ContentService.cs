using Microsoft.EntityFrameworkCore;
using revisa_api.Data.content;
using Grade = revisa_api.Data.content.Grade;

public class ContentService : IContentService
{
    private readonly ContentContext _dbContext;

    public ContentService(ContentContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Inserts content info to the database
    // returning the id of the newly created or updated entity
    public PostContentInfoResponse PostContentInfo(PostContentBaseRequest request)
    {
        // using var context = _dbContext;
        //prepare meta data

        ContentDetail cd =
            _dbContext.ContentDetails.FirstOrDefault(c =>
                c.Client.ClientName == request.Info.Client
                && c.Grade.Grade1 == request.Info.Grade
                && c.Subject.Subject1 == request.Info.Subject
                && c.DeliveryDate == DateOnly.Parse(request.Info.DeliveryDate)
            ) ?? new();

        _dbContext.Add(cd);

        cd.Client =
            _dbContext.Clients.FirstOrDefault(c => c.ClientName == request.Info.Client)
            ?? new Client { ClientName = request.Info.Client };

        cd.Subject =
            _dbContext.Subjects.FirstOrDefault(s => s.Subject1 == request.Info.Subject)
            ?? new Subject { Subject1 = request.Info.Subject };

        cd.Grade =
            _dbContext.Grades.FirstOrDefault(g => g.Grade1 == request.Info.Grade)
            ?? new Grade { Grade1 = request.Info.Grade };

        cd.Language =
            _dbContext.ContentLanguages.FirstOrDefault(l =>
                l.Language == request.Info.Language.ToUpper()
            ) ?? new ContentLanguage { Language = request.Info.Language };

        cd.Owner =
            _dbContext.Users.FirstOrDefault(u => u.Email == request.Info.UpdatedBy.Email) ?? new();

        cd.Status =
            _dbContext.ContentStatuses.FirstOrDefault(s => s.Status == request.Info.Status)
            ?? new ContentStatus { Id = 0 };

        cd.File =
            _dbContext.ContentFiles.FirstOrDefault(f => f.FileId == request.Info.File.FileId)
            ?? new ContentFile
            {
                FileId = request.Info.File.FileId,
                FileName = request.Info.File.FileName,
                CreatedAt = request.Info.File.CreatedAt,
            };

        _dbContext.SaveChanges();

        ContentTranslation translation = _dbContext.ContentTranslations.FirstOrDefault(t =>
            t.ContentLanguage.Language == request.Info.Language.ToUpper()
            && t.ContentSubject.Subject1.ToUpper() == request.Info.Subject.ToUpper()
            && t.ContentGrade.Grade1.ToUpper() == request.Info.Grade.ToUpper()
        );

        if (translation != null)

        return new PostContentInfoResponse {ContentId = cd.Id, NeedsTranslation = translation != null ? true : false, Status = cd.Status.Status};

        // ContentDetail cd =
        //     _dbContext.ContentDetails.FirstOrDefault(c =>
        //         c.Client.ClientName == request.Info.Client
        //         && c.Grade.Grade1 == request.Info.Grade
        //         && c.Subject.Subject1 == request.Info.Subject
        //         && c.DeliveryDate == DateOnly.Parse(request.Info.DeliveryDate)
        //     )
        //     ?? new ContentDetail
        //     {
        //         Client = client,
        //         Grade = grade,
        //         Subject = subject,
        //         Owner = owner,
        //         File = file,
        //         Status = status,
        //         DeliveryDate = DateOnly.Parse(request.Info.DeliveryDate),
        //         CreatedAt = DateTime.Parse(request.Info.CreatedAt),
        //         UpdatedAt = DateTime.Now
        //     };
        // if (cd.Id == 0) { }
        // MapContentDetails(cd, request, client, subject, file, _dbContext);

        // if (cd == null)
        // {
        //     cd = new ContentDetail();
        //     _dbContext.Add(cd);
        //     MapContentDetails(cd, request, client, subject, file, _dbContext);
        // }
        // else
        // {
        //     _dbContext.Update(cd);
        // MapContentDetails(cd, request, client, subject, file, _dbContext);
        // }
    }

    public int PostContent(PostContentRequest request)
    {
        // using var context = _dbContext;

        // ContentDetail cd = PostContentInfo(request);

        ContentDetail cd = new ();

        // prepare content version
        ContentVersion? contentVersion =
            _dbContext
                .ContentVersions.Include(v => v.ContentGroups)
                .FirstOrDefault(cv => cv.ContentDetailsId == cd.Id && cv.IsLatest == 1)
            ?? new ContentVersion { ContentDetailsId = cd.Id };

        // // add language supports and standards
        // List<TeksItem> teksItems = _teksService.GetTeksItems(
        //     request.Info.Teks,
        //     cd.Grade.Grade1,
        //     cd.Subject
        // );
        // LessonSchedule lessonSchedule = _languageSupportService.GetLessonSchedule(cd.DeliveryDate);
        // StrategyObjective strategy_objective = _elpsService.GetStrategyObjective(
        //     lessonSchedule.LessonOrder
        // );

        // Iclo iclo = _languageSupportService.GetIclo(teksItems, lessonSchedule, strategy_objective);

        // map slide content
        foreach (var slide in request.Content)
        {
            ContentGroup slideElements = new();
            foreach (var element in slide)
            {
                slideElements.ContentTxts.Add(
                    new ContentTxt { Txt = element.TextContent, ObjectId = element.ObjectId }
                );
            }

            if (slideElements != null)
            {
                contentVersion.ContentGroups.Add(slideElements);
            }
        }
        _dbContext.SaveChanges();
        return cd.Id;
    }

    // private void MapContentDetails(
    //     ContentDetail cd,
    //     PostContentBaseRequest request,
    //     Client client,
    //     Subject subject,
    //     revisa_api.Data.content.ContentFile file,
    //     ContentContext context
    // )
    // {
    //     cd.Client = client;
    //     cd.GradeId = context.Grades.FirstOrDefault(g => g.Grade1 == request.Info.Grade).Id;
    //     cd.Subject = subject;
    //     cd.Owner = new revisa_api.Data.content.User
    //     {
    //         Username = request.Info.UpdatedBy.Username,
    //         Email = request.Info.UpdatedBy.Email
    //     };
    //     cd.File = file;
    //     cd.Status = context.ContentStatuses.FirstOrDefault(s => s.Status == request.Info.Status);
    //     cd.DeliveryDate = DateOnly.Parse(request.Info.DeliveryDate);
    //     cd.CreatedAt = DateTime.Parse(request.Info.CreatedAt);
    //     cd.UpdatedAt = DateTime.Now;
    // }

    public GetContentResponse GetContent(int contentId)
    {
        using var context = _dbContext;
        ContentDetail? entity = GetContentDetail(contentId);

        if (entity == null)
        {
            return new GetContentResponse(null);
        }

        GetContentResponse response = new(entity);

        return response;
    }

    public GetContentBaseResponse GetContentInfo(int contentId)
    {
        using var context = _dbContext;
        ContentDetail? entity = GetContentDetail(contentId);

        return new(entity);
    }

    public List<GetContentBaseResponse> GetContentInfoBySubject(string subject)
    {
        List<GetContentBaseResponse> contentInfoList = new();

        using var context = _dbContext;
        List<ContentDetail> contentDetailList = _dbContext
            .ContentDetails.Include(c => c.Client)
            .Include(c => c.Grade)
            .Include(c => c.Subject)
            .Include(c => c.Owner)
            .Include(c => c.File)
            .Include(c => c.Status)
            .Where(c => c.Subject.Subject1 == subject.ToUpper())
            .ToList();

        contentDetailList.ForEach(c => contentInfoList.Add(new(entity: c)));

        return contentInfoList;
    }

    private ContentDetail? GetContentDetail(int contentId)
    {
        using var context = _dbContext;
        return context
            .ContentDetails.Include(c => c.Client)
            .Include(c => c.Grade)
            .Include(c => c.Subject)
            .Include(c => c.Owner)
            .Include(c => c.File)
            .Include(c => c.ContentVersions)
            .ThenInclude(v => v.ContentGroups)
            .ThenInclude(g => g.ContentTxts)
            .Include(c => c.Status)
            .FirstOrDefault(c => c.Id == contentId);
    }
}
