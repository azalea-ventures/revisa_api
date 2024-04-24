using Microsoft.EntityFrameworkCore;
using revisa_api.Data;

public class ContentService : IContentService
{
    public int PostContent(PostContentRequest request)
    {
        using var context = new RevisaDbContext();
        using var transaction = context.Database.BeginTransaction();

        //prepare meta data
        Client client =
            context.Clients.FirstOrDefault(c => c.ClientName == request.Info.Client)
            ?? new Client { ClientName = request.Info.Client };

        Subject subject =
            context.Subjects.FirstOrDefault(s => s.Subject1 == request.Info.Subject)
            ?? new Subject { Subject1 = request.Info.Subject };

        ContentDetail cd =
            context.ContentDetails.FirstOrDefault(c =>
                c.Client.ClientName == request.Info.Client 
                && c.Grade.Grade1 == request.Info.Grade 
                && c.Subject.Subject1 == request.Info.Subject 
                && c.DeliveryDate == DateOnly.Parse(request.Info.DeliveryDate))
                ?? new ContentDetail();

        _ = context.Add<ContentDetail>(cd);

        cd.Client = client;
        cd.GradeId = context.Grades.FirstOrDefault(g => g.Grade1 == request.Info.Grade).Id;
        cd.Subject = subject;
        cd.Owner = new revisa_api.Data.User
        {
            Username = request.Info.UpdatedBy.Username,
            Email = request.Info.UpdatedBy.Email
        };
        // TODO: filename blocked - app script dev needed
        cd.OriginalFilename = "";
        cd.DeliveryDate = DateOnly.Parse(request.Info.DeliveryDate);
        cd.CreatedAt = DateTime.Parse(request.Info.CreatedAt);
        cd.UpdatedAt = DateTime.Now;

        context.SaveChanges();
        // prepare content data
        ContentVersion? contentVersion = context
            .ContentVersions.Include(v => v.ContentGroups)
            .FirstOrDefault(cv => cv.ContentDetailsId == cd.Id && cv.IsLatest == 1) 
            ?? new ContentVersion{ContentDetailsId = cd.Id};

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
        context.SaveChanges();
        transaction.Commit();
        return cd.Id;
    }

    public GetContentResponse GetContent(int contentId)
    {
        using var context = new RevisaDbContext();
        ContentDetail? entity = GetContentDetail(contentId);

        if (entity == null)
        {
            return new GetContentResponse();
        }

        GetContentResponse response = new(entity);

        return response;
    }

    private ContentDetail? GetContentDetail(int contentId)
    {
        using var context = new RevisaDbContext();
        return context
            .ContentDetails.Include(c => c.Client)
            .Include(c => c.Grade)
            .Include(c => c.Subject)
            .Include(c => c.Owner)
            .Include(c => c.ContentVersions)
            .ThenInclude(v => v.ContentGroups)
            .ThenInclude(g => g.ContentTxts)
            .FirstOrDefault();
    }
}
