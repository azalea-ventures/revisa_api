using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using revisa_api.Data;

public class ContentService : IContentService
{

    public int PostContent(PostContentRequest request)
    {
        using var context = new RevisaDbContext();
        ContentDetail cd = new()
        {
            Client = new Client
            {
                ClientName = request.Info.Client
            },
            Grade = new Grade
            {
                Grade1 = request.Info.Grade
            },
            Subject = new Subject
            {
                Subject1 = request.Info.Subject
            },
            Owner = new revisa_api.Data.User
            {
                Username = request.Info.UpdatedBy.Username,
                Email = request.Info.UpdatedBy.Email
            },
            // TODO: filename blocked by app script
            OriginalFilename = "",
            DeliveryDate = DateOnly.Parse(request.Info.DeliveryDate),
            CreatedAt = DateTime.Parse(request.Info.CreatedAt),
            UpdatedAt = DateTime.Now
        };

        _ = context.Add<ContentDetail>(cd);

        context.SaveChanges();

        ContentVersion? contentVersion = context.ContentVersions
        .Include(v => v.ContentGroups)
        .FirstOrDefault(cv => cv.ContentDetailsId == cd.Id && cv.IsLatest == 1);

        foreach (var slide in request.Content)
        {
            ContentGroup slideElements = new();
            foreach (var element in slide)
            {
                slideElements.ContentTxts.Add(new ContentTxt { Txt = element.TextContent, ObjectId = element.ObjectId });
            }

            if (slideElements != null)
            {
                contentVersion.ContentGroups.Add(slideElements);
            }
        }
        context.SaveChanges();

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
        return context.ContentDetails
        .Include(c => c.Client)
        .Include(c => c.Grade)
        .Include(c => c.Subject)
        .Include(c => c.Owner)
        .Include(c => c.ContentVersions)
        .ThenInclude(v => v.ContentGroups)
        .ThenInclude(g => g.ContentTxts)
        .FirstOrDefault();

    }
}