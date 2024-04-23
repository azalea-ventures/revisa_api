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

        int id = context.SaveChanges();

        ContentVersion? contentVersion = cd.ContentVersions.FirstOrDefault(cv => cv.IsLatest == 1);

        foreach (var slide in request.Content){
            ContentTxt contentTxt = new{Content = slide.};


        }
        contentVersion.ContentTxts

        return id;
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

    private ContentDetail? GetContentDetail(int contentId){
        using var context = new RevisaDbContext();
        return context.ContentDetails
        .Include(c => c.Client)
        .Include(c => c.Grade)
        .Include(navigationPropertyPath: c => c.Subject)
        .Include(c => c.Owner)
        .Include(c => c.ContentVersions)
        .FirstOrDefault();

    }
}