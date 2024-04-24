using System.Text.Json.Serialization;

public class Info
{

    public int Id { get; set; } = 0;
    public string Client { get; set; }
    public string Grade { get; set; }
    public string Subject { get; set; }
    [JsonPropertyName("delivery_date")]
    public string DeliveryDate { get; set; }
    [JsonPropertyName("updated_by")]
    public User UpdatedBy { get; set; } = new();
    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }
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

public class GetContentResponse{

    // Specifically for using with mapping entity objects to responses
    public GetContentResponse(revisa_api.Data.ContentDetail entity)
    {
        Info.Client = entity.Client.ClientName;
        Info.Grade = entity.Grade.Grade1;
        Info.Subject = entity.Subject.Subject1;
        Info.DeliveryDate = entity.DeliveryDate.ToString();
        Info.CreatedAt = entity.CreatedAt.ToString();
        Info.UpdatedBy.Email = entity.Owner.Email;
        Info.UpdatedBy.Username = entity.Owner.Username.ToString();

        foreach (var group in entity.ContentVersions.FirstOrDefault().ContentGroups){
            List<Content> contentList = [];
            foreach (var txt in group.ContentTxts){
                contentList.Add(new Content{ObjectId=txt.ObjectId, TextContent=txt.Txt});
            }
            Content.Add(contentList);
        }
    }

    public GetContentResponse(){}

    public Info Info { get; set; } = new ();
    public List<List<Content>>? Content{ get; set;} = new ();
}