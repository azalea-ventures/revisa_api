using System.Text.Json.Serialization;

public class Info
{
    public int Id { get; set; } = 0;
    public string Client { get; set; }
    public string Grade { get; set; }
    public string Subject { get; set; }
    public List<string> Teks { get; set; }
    public ContentFile File { get; set; }

    [JsonPropertyName("delivery_date")]
    public string DeliveryDate { get; set; }

    [JsonPropertyName("updated_by")]
    public User UpdatedBy { get; set; } = new();

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }
}

public class ContentFile()
{
    public Guid Id { get; set; }

    public string? FileName { get; set; }

    public Guid? SourceFileId { get; set; }

    public string? CurrentFolderId { get; set; }

    public Guid? OutbountFileId { get; set; }

    public Guid? OutboundFolderId { get; set; }

    public string? OutboundPath { get; set; }

    public DateTime? CreatedAt { get; set; }
}

public class Content()
{
    [JsonPropertyName("object_id")]
    public string ObjectId { get; set; }

    [JsonPropertyName("text_content")]
    public string TextContent { get; set; }
}

public class PostContentRequest
{
    public required Info Info { get; set; }
    public required List<List<Content>> Content { get; set; }
}

public class PostContentInfoRequest{
    public required Info Info { get; set;}
}

public class PostContentResponse
{
    [JsonPropertyName("elps_strategy")]
    public string ElpsStrategy { get; set; }

    [JsonPropertyName("elps_strategy_icon_id")]
    public string ElpsStrategyIconId { get; set; }

    [JsonPropertyName("elps_domain_objective")]
    public string ElpsDomainObjective { get; set; }

    [JsonPropertyName("teks")]
    public string Teks { get; set; }
}

public class GetContentResponse
{
    // Specifically for using with mapping entity objects to responses
    public GetContentResponse(revisa_api.Data.content.ContentDetail entity)
    {
        Info.Client = entity.Client.ClientName;
        Info.Grade = entity.Grade.Grade1;
        Info.Subject = entity.Subject.Subject1;
        Info.DeliveryDate = entity.DeliveryDate.ToString();
        Info.CreatedAt = entity.CreatedAt.ToString();
        Info.UpdatedBy.Email = entity.Owner.Email;
        Info.UpdatedBy.Username = entity.Owner.Username.ToString();

        foreach (var group in entity.ContentVersions.FirstOrDefault().ContentGroups)
        {
            List<Content> contentList = [];
            foreach (var txt in group.ContentTxts)
            {
                contentList.Add(new Content { ObjectId = txt.ObjectId, TextContent = txt.Txt });
            }
            Content.Add(contentList);
        }
    }

    public GetContentResponse() { }

    public Info Info { get; set; } = new();
    public List<List<Content>>? Content { get; set; } = new();
}
