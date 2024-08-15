using Microsoft.EntityFrameworkCore;
using revisa_api.Data.content;
using revisa_api.Data.elps;
using revisa_api.Data.language_supports;
using revisa_api.Data.teks;

public class ContentService : IContentService
{
    private readonly ContentContext _dbContext;
    private readonly ITeksService _teksService;
    private readonly ILanguageSupportService _languageSupportService;
    private readonly IElpsService _elpsService;

    public ContentService(
        ContentContext dbContext,
        ITeksService teksService,
        ILanguageSupportService languageSupportService,
        IElpsService elpsService
    )
    {
        _dbContext = dbContext;
        _teksService = teksService;
        _languageSupportService = languageSupportService;
        _elpsService = elpsService;
    }

    // Inserts content info to the database
    // returning the id of the newly created or updated entity
    public ContentDetail PostContentInfo(PostContentBaseRequest request)
    {
        // using var context = _dbContext;
        //prepare meta data
        Client client =
            _dbContext.Clients.FirstOrDefault(c => c.ClientName == request.Info.Client)
            ?? new Client { ClientName = request.Info.Client };

        Subject subject =
            _dbContext.Subjects.FirstOrDefault(s => s.Subject1 == request.Info.Subject)
            ?? new Subject { Subject1 = request.Info.Subject };

        ContentDetail cd = _dbContext.ContentDetails.FirstOrDefault(c =>
            c.Client.ClientName == request.Info.Client
            && c.Grade.Grade1 == request.Info.Grade
            && c.Subject.Subject1 == request.Info.Subject
            && c.DeliveryDate == DateOnly.Parse(request.Info.DeliveryDate)
        );

        revisa_api.Data.content.ContentFile file =
            _dbContext.ContentFiles.FirstOrDefault(f => f.FileId == request.Info.File.FileId)
            ?? new revisa_api.Data.content.ContentFile
            {
                Id = Guid.NewGuid(),
                FileId = request.Info.File.FileId,
                FileName = request.Info.File.FileName,
                CreatedAt = request.Info.File.CreatedAt,
            };

        if (cd == null)
        {
            cd = new ContentDetail();
            _dbContext.Add(cd);
            MapContentDetails(cd, request, client, subject, file, _dbContext);
        }
        else
        {
            _dbContext.Update(cd);
            MapContentDetails(cd, request, client, subject, file, _dbContext);
        }

        _dbContext.SaveChanges();
        return cd;
    }

    public int PostContent(PostContentRequest request)
    {
        // using var context = _dbContext;

        ContentDetail cd = PostContentInfo(request);

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

    private void MapContentDetails(
        ContentDetail cd,
        PostContentBaseRequest request,
        Client client,
        Subject subject,
        revisa_api.Data.content.ContentFile file,
        ContentContext context
    )
    {
        cd.Client = client;
        cd.GradeId = context.Grades.FirstOrDefault(g => g.Grade1 == request.Info.Grade).Id;
        cd.Subject = subject;
        cd.Owner = new revisa_api.Data.content.User
        {
            Username = request.Info.UpdatedBy.Username,
            Email = request.Info.UpdatedBy.Email
        };
        cd.File = file;
        cd.Status = context.ContentStatuses.FirstOrDefault(s => s.Status == request.Info.Status);
        cd.DeliveryDate = DateOnly.Parse(request.Info.DeliveryDate);
        cd.CreatedAt = DateTime.Parse(request.Info.CreatedAt);
        cd.UpdatedAt = DateTime.Now;
    }

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
