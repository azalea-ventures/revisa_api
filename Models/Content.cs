using System.Text.Json.Serialization;

public class Info
{
    public int Id { get; set; }
    public string Client { get; set; }
    public string Grade { get; set; }
    public string Subject { get; set; }
    public string Language { get; set; }
    public List<string> Teks { get; set; }
    public File File { get; set; }
    public string Status { get; set; }

    [JsonPropertyName("delivery_date")]
    public string DeliveryDate { get; set; }

    [JsonPropertyName("updated_by")]
    public User UpdatedBy { get; set; } = new();

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }
}

public class File()
{
    [JsonPropertyName("file_id")]
    public string? FileId { get; set; }

    [JsonPropertyName("file_name")]
    public string? FileName { get; set; }

    [JsonPropertyName("source_file_id")]
    public string? SourceFileId { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class Content()
{
    [JsonPropertyName("object_id")]
    public string ObjectId { get; set; }

    [JsonPropertyName("text_content")]
    public string TextContent { get; set; }
}

public class PostContentBaseRequest
{
    public required Info Info { get; set; }
}

public class PostContentRequest : PostContentBaseRequest
{
    public required List<List<Content>> Content { get; set; }
}

public class PostContentInfoResponse
{
    [JsonPropertyName("content_id")]
    public int ContentId { get; set; }

    [JsonPropertyName("needs_translation")]
    public bool NeedsTranslation { get; set; }
    public string Status { get; set; }
    
}

public class PostContentResponse
{
    [JsonPropertyName("elps_strategy")]
    public string ElpsStrategy { get; set; }

    [JsonPropertyName("elps_strategy_icon_id")]
    public string ElpsStrategyIconId { get; set; }

    [JsonPropertyName("elps_strategy_file_id")]
    public string ElpsStrategyFileId { get; set; }

    [JsonPropertyName("elps_domain_objective")]
    public string ElpsDomainObjective { get; set; }

    [JsonPropertyName("elps_domain_name")]
    public string ElpsDomainName { get; set; }
    public int ElpsStrategyId { get; set; }

    [JsonPropertyName("elps_strategy_label")]
    public string ElpsStrategyLabel { get; set; }

    [JsonPropertyName("teks")]
    public string Teks { get; set; }
}

public class GetContentBaseResponse
{
    public Info Info { get; set; } = new();

    // Specifically for using with mapping entity objects to responses
    public GetContentBaseResponse(revisa_api.Data.content.ContentDetail entity)
    {
        Info.Id = entity.Id;
        Info.Client = entity.Client.ClientName;
        Info.Grade = entity.Grade.Grade1;
        Info.Subject = entity.Subject.Subject1;
        Info.DeliveryDate = entity.DeliveryDate.ToString();
        Info.Language = entity.Language.Language;
        Info.File = new()
        {
            FileId = entity.File?.FileId,
            FileName = entity.File?.FileName,
            CreatedAt = entity.File?.CreatedAt,
        };
        Info.Status = entity.Status?.Status;
        Info.CreatedAt = entity.CreatedAt?.ToString();
        Info.UpdatedBy.Email = entity.Owner.Email;
        Info.UpdatedBy.Username = entity.Owner.Username.ToString();
    }
}

public class GetContentResponse : GetContentBaseResponse
{
    public List<List<Content>>? Content { get; set; } = new();

    public GetContentResponse(revisa_api.Data.content.ContentDetail entity)
        : base(entity)
    {
        var latestVersion = entity.ContentVersions.Where(cv => cv.IsLatest == 1).FirstOrDefault();
        foreach (var group in latestVersion.ContentGroups)
        {
            List<Content> contentList = [];
            foreach (var txt in group.ContentTxts)
            {
                contentList.Add(new Content { ObjectId = txt.ObjectId, TextContent = txt.Txt });
            }
            Content.Add(contentList);
        }
    }
}


public class PutContentInfoRequest{
    [JsonPropertyName("content_id")]
    public int ContentId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
}
