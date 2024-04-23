using System.Text.Json.Serialization;

public class User{
    [JsonPropertyName("name")]
    public string Username {get; set;}
    [JsonPropertyName("email")]
    public string Email {get; set;}
}