using System.Text.Json.Serialization;

class TranslationResponse
{
    [JsonPropertyName("translations")]
    public List<Translation> Translations { get; set; }
}

public class Translation
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("to")]
    public string To { get; set; }

    [JsonPropertyName("alignment")]
    public Alignment Alignment { get; set; }
}

public class Alignment
{
    [JsonPropertyName("proj")]
    public string Proj { get; set; }
}

public class TranslateContent{
    public string ObjectId { get; set; }
    public int OriginalIndex { get; set; }
    public object TxtObj { get; set; }
}
