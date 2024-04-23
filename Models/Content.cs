using System.Text.Json.Serialization;

public class Info
{

    public string Client { get; set; }
    public string Grade { get; set; }
    public string Subject { get; set; }
    [JsonPropertyName("delivery_date")]
    public string DeliveryDate { get; set; }
    [JsonPropertyName("updated_by")]
    public User UpdatedBy { get; set; }
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