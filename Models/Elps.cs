
using System.Text.Json.Serialization;

public class ElpsSupportResponse{

    [JsonPropertyName("elps_strategy")]
    public string ElpsStrategy { get; set; }
    [JsonPropertyName("elps_strategy_icon_id")]
    public string ElpsStrategyIconId {get; set; }
    [JsonPropertyName("elps_domain_objective")]
    public string ElpsDomainObjective { get; set; }
    [JsonPropertyName("elps_strategy_id")]
    public int ElpsStrategyId { get; set; }
    [JsonPropertyName("elps_strategy_label")]
    public string ElpsStrategyLabel { get; set; }
    [JsonPropertyName("elps_strategy_file_id")]
    public string ElpsStrategyFileId { get; set; }
    [JsonPropertyName("elps_domain_name")]
    public string ElpsDomainName {get; set; }
    [JsonPropertyName("teks")]
    public string Teks{ get; set; }
}