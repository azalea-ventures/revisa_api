using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConversationModel
{
    public class Settings
    {
        [JsonPropertyName("confidenceThreshold")]
        public int ConfidenceThreshold { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("projectKind")]
        public string ProjectKind { get; set; }

        [JsonPropertyName("projectName")]
        public string ProjectName { get; set; }

        [JsonPropertyName("multilingual")]
        public bool Multilingual { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("settings")]
        public Settings Settings { get; set; }
    }

    public class Synonyms
    {
        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("values")]
        public List<string> Values { get; set; }
    }

    public class Sublists
    {
        [JsonPropertyName("listKey")]
        public string ListKey { get; set; }

        [JsonPropertyName("synonyms")]
        public List<Synonyms> Synonyms { get; set; }
    }

    public class List
    {
        [JsonPropertyName("sublists")]
        public List<Sublists> Sublists { get; set; }
    }

    public class RegexExpression
    {
        [JsonPropertyName("regexKey")]
        public string RegexKey { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("regexPattern")]
        public string RegexPattern { get; set; }
    }

    public class Entity
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("compositionSetting")]
        public string CompositionSetting { get; set; }

        [JsonPropertyName("list")]
        public List List { get; set; }

        [JsonPropertyName("prebuilts")]
        public List<Prebuilt> Prebuilts { get; set; }

        [JsonPropertyName("regex")]
        public List<RegexExpression> RegexExpressions { get; set; }

        [JsonPropertyName("requiredComponents")]
        public List<string> RequiredComponents { get; set; }
    }

    public class Prebuilt
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }
    }

    public class Intent
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }
    }

    public class EntityInstance
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }
    }

    public class Utterance
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("intent")]
        public string Intent { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("dataset")]
        public string Dataset { get; set; }

        [JsonPropertyName("entities")]
        public List<EntityInstance> Entities { get; set; }
    }

    public class Assets
    {
        [JsonPropertyName("projectKind")]
        public string ProjectKind { get; set; }

        [JsonPropertyName("intents")]
        public List<Intent> Intents { get; set; }

        [JsonPropertyName("entities")]
        public List<Entity> Entities { get; set; }

        [JsonPropertyName("utterances")]
        public List<Utterance> Utterances { get; set; }
    }

    public class ConversationModel
    {
        [JsonPropertyName("projectFileVersion")]
        public string ProjectFileVersion { get; set; }

        [JsonPropertyName("stringIndexType")]
        public string StringIndexType { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("assets")]
        public Assets Assets { get; set; }
    }
}
