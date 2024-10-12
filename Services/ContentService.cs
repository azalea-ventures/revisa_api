using Microsoft.EntityFrameworkCore;
using revisa_api.Data.content;
using Grade = revisa_api.Data.content.Grade;

public interface IContentService{

    PostContentInfoResponse PostContentInfo(PostContentBaseRequest request);
    int PostContent(PostContentRequest content);
    GetContentResponse GetContent(int contentId);
    GetContentBaseResponse GetContentInfo(int contentId);

    List<GetContentBaseResponse> GetContentInfoBySubject(string subject);
}

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
        ContentDetail cd =
            _dbContext.ContentDetails.FirstOrDefault(c =>
                c.Grade.Grade1 == request.Info.Grade
                && c.Subject.Subject1 == request.Info.Subject
                && c.DeliveryDate == DateOnly.Parse(request.Info.DeliveryDate)
            ) ?? new();

        _dbContext.Update(cd);

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
                l.Abbreviation.Equals(
                    request.Info.Language)
            ) ?? new ContentLanguage { Abbreviation = request.Info.Language };

        cd.Owner =
            _dbContext.Users.FirstOrDefault(u => u.Email == request.Info.UpdatedBy.Email)
            ?? new revisa_api.Data.content.User
            {
                Username = request.Info.UpdatedBy.Username,
                Email = request.Info.UpdatedBy.Email
            };

        cd.Status =
            _dbContext.ContentStatuses.FirstOrDefault(s => s.Status == request.Info.Status)
            ?? new ContentStatus { Id = 0 };

        cd.File =
            _dbContext.ContentFiles.FirstOrDefault(f => f.FileId == request.Info.File.FileId)
            ?? new ContentFile
            {
                FileId = request.Info.File.FileId,
                FileName = request.Info.File.FileName,
                SourceFileId = request.Info.File.SourceFileId,
                CreatedAt = request.Info.File.CreatedAt,
            };

        cd.DeliveryDate = DateOnly.Parse(request.Info.DeliveryDate);

        _dbContext.SaveChanges();

        ContentTranslation translation = _dbContext.ContentTranslations.FirstOrDefault(t =>
            t.ContentLanguage.Abbreviation.Equals(
                request.Info.Language)
            && t.ContentSubject.Subject1.Equals(
                request.Info.Subject)
            && t.ContentGrade.Grade1.Equals(
                request.Info.Grade)
        );

        return new PostContentInfoResponse
        {
            ContentId = cd.Id,
            NeedsTranslation = translation != null ? true : false,
            Status = cd.Status.Status,
        };
    }

    public int PostContent(PostContentRequest request)
    {
        // prepare content version
        ContentVersion? contentVersion =
            _dbContext
                .ContentVersions.Include(v => v.ContentGroups)
                .FirstOrDefault(cv => cv.ContentDetailsId == request.Info.Id && cv.IsLatest == 1)
            ?? new ContentVersion { ContentDetailsId = request.Info.Id };

        if (contentVersion != null)
        {
            contentVersion.IsLatest = 0;
            _dbContext.SaveChanges();
            contentVersion = new ContentVersion { ContentDetailsId = request.Info.Id };
            _dbContext.Add(contentVersion);
        }

        contentVersion.Owner = _dbContext.Users.FirstOrDefault(u =>
            u.Email.ToUpper().Equals(request.Info.UpdatedBy.Email)
        );

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
        return contentVersion.Id;
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
            .Include(c => c.Language)
            .Include(c => c.File)
            .Include(c => c.Status)
            .Where(c =>
                c.Subject.Subject1.Equals(subject)
            )
            .ToList();

        contentDetailList.ForEach(c => contentInfoList.Add(new(entity: c)));

        return contentInfoList;
    }

    private ContentDetail? GetContentDetail(int contentId)
    {
        return _dbContext
            .ContentDetails.Include(c => c.Client)
            .Include(c => c.Grade)
            .Include(c => c.Subject)
            .Include(c => c.Owner)
            .Include(c => c.Language)
            .Include(c => c.File)
            .Include(c => c.ContentVersions)
            .ThenInclude(v => v.ContentGroups)
            .ThenInclude(g => g.ContentTxts)
            .Include(c => c.Status)
            .FirstOrDefault(c => c.Id == contentId);
    }
}
