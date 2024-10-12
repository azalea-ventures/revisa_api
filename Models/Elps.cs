using System.Text.Json.Serialization;

public class ElpsSupportResponse
{
    [JsonPropertyName("package_meta")]
    public SupportPackageMeta PackageMeta { get; set; }

    [JsonPropertyName("elps_strategy")]
    public string ElpsStrategy { get; set; }

    [JsonPropertyName("elps_domain_name")]
    public string ElpsDomainName { get; set; }

    [JsonPropertyName("elps_domain_objective")]
    public string ElpsObjective { get; set; }

    [JsonPropertyName("elps_strategy_label")]
    public string ElpsStrategyLabel { get; set; }

    [JsonPropertyName("elps_strategy_file_id")]
    public string ElpsStrategyFileId { get; set; }

    [JsonPropertyName("elps_strategy_icon_id")]
    public string ElpsStrategyIconId { get; set; }
}

public class SupportPackageMeta
{
    [JsonPropertyName("delivery_date")]
    public string deliveryDate { get; set; }

    [JsonPropertyName("grade")]
    public string grade { get; set; }

    [JsonPropertyName("subject")]
    public string subject { get; set; }
}

public class ElpsSupportsRequest
{
    [JsonPropertyName("domain_objective_data")]
    public List<DomainObjectiveMedia> DomainObjectiveData { get; set; }

    [JsonPropertyName("strategy_data")]
    public List<StrategyMedia> StrategyData { get; set; }
}

public class DomainObjectiveMedia
{
    [JsonPropertyName("rich_text")]
    public List<RichTextMedia> RichText { get; set; }

    public string Domain { get; set; }

    [JsonPropertyName("domain_objective_label")]
    public string DomainObjectiveLabel { get; set; }

    [JsonPropertyName("domain_objective_statement")]
    public string DomainObjectiveStatement { get; set; }
}

public class StrategyMedia
{
    [JsonPropertyName("rich_text")]
    public List<RichTextMedia> RichText { get; set; }

    [JsonPropertyName("strategy_label")]
    public string StrategyLabel { get; set; }

    [JsonPropertyName("strategy_statement")]
    public string StrategyStatement { get; set; }
}

public class RichTextMedia
{
    [JsonPropertyName("is_bold")]
    public bool IsBold { get; set; }
    public string Text { get; set; }
}
