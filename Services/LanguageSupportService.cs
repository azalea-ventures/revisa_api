// using System.Net.Http;
// using System.Text.Json;
// using Microsoft.Net.Http.Headers;

using Azure;
using revisa_api.Data.language_supports;

public class LanguageSupportService : ILanguageSupportService{

    private readonly LanguageSupportContext _dbContext;

    public LanguageSupportService(LanguageSupportContext dbContext){
        _dbContext = dbContext;
    }

    public ElpsSupportResponse GetElpsSupports(string delivery_date){
        using var dbContext = _dbContext;
        var lesson_schedule = dbContext.LessonSchedules.FirstOrDefault(s => s.DeliveryDate == DateOnly.Parse(delivery_date));

        if (lesson_schedule == null){
            return new();
        }

        //TODO: this is sloppy, refactor
        var iclo = dbContext.Iclos.Where(i => i.LessonScheduleId == lesson_schedule.Id).ToList()[0];
        var strategy_objective = dbContext.StrategiesObjectives.FirstOrDefault(d => d.Id == iclo.StrategyObjectiveId);
        var strategy = dbContext.LearningStrategiesMods.FirstOrDefault(s => s.Id == strategy_objective.StrategyModId);
        var domain_objective = dbContext.DomainObjectives.FirstOrDefault(d => d.Id == strategy_objective.DomainObjectiveId);

        var teks_item = dbContext.TeksItems.FirstOrDefault(t => t.Id == iclo.TeksItemId);

        var response = new ElpsSupportResponse
        {
            ElpsStrategy = strategy.Strategy,
            ElpsDomainObjective = $"({domain_objective.Label}) " + domain_objective.Objective,
            ElpsStrategyIconId = strategy.ImageFileId,
            Teks = $"({teks_item.HumanCodingScheme}) " + teks_item.FullStatement,
        };

        return response;
    }
}

//     // private readonly ContentContext _dbContext;
//     private readonly IHttpClientFactory _httpClientFactory;

//     public LanguageSupportService(
//         // IDbContextFactory<TeksContext> dbContextFactory,
//         IHttpClientFactory httpClientFactory
//     )
//     {
//         // _dbContextFactory = dbContextFactory;
//         _httpClientFactory = httpClientFactory;
//     }


//     private async Task OnPost(string endpoint)
//     {
//         var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint)
//         {
//             Headers =
//             {
//                 { HeaderNames.Accept, "application/json" },
//             }
//         };

//         var httpClient = _httpClientFactory.CreateClient();

//         var res = await httpClient
//             .SendAsync(httpRequestMessage)
//             .ContinueWith(
//                 (res) =>
//                 {
//                     return JsonSerializer.Deserialize<TEKSResponse>(
//                         res.Result.Content.ReadAsStream()
//                     );
//                 }
//             );

//         return;
//     }

//     private ConversationModel GetConversationModel(string conversationId){

//         var conversationModel = new ConversationModel
//             {
//                 ProjectFileVersion = "2022-10-01-preview",
//                 StringIndexType = "Utf16CodeUnit",
//                 Metadata = new Metadata
//                 {
//                     ProjectKind = "Conversation",
//                     ProjectName = "{PROJECT-NAME}",
//                     Multilingual = true,
//                     Description = "DESCRIPTION",
//                     Language = "{LANGUAGE-CODE}",
//                     Settings = new Settings
//                     {
//                         ConfidenceThreshold = 0
//                     }
//                 },
//                 Assets = new Assets
//                 {
//                     ProjectKind = "Conversation",
//                     Intents = new List<Intent>
//                     {
//                         new Intent
//                         {
//                             Category = "intent1"
//                         }
//                     },
//                     Entities = new List<Entity>
//                     {
//                         new Entity
//                         {
//                             Category = "entity1",
//                             CompositionSetting = "{COMPOSITION-SETTING}",
//                             List = new List
//                             {
//                                 Sublists = new List<Sublists>
//                                 {
//                                     new Sublists
//                                     {
//                                         ListKey = "list1",
//                                         Synonyms = new List<Synonyms>
//                                         {
//                                             new Synonyms
//                                             {
//                                                 Language = "{LANGUAGE-CODE}",
//                                                 Values = new List<string> { "{VALUES-FOR-LIST}" }
//                                             }
//                                         }
//                                     }
//                                 }
//                             },
//                             Prebuilts = new List<Prebuilt>
//                             {
//                                 new Prebuilt
//                                 {
//                                     Category = "{PREBUILT-COMPONENTS}"
//                                 }
//                             },
//                             RegexExpressions = new List<RegexExpression>
//                             {
//                                 new RegexExpression
//                                 {
//                                     RegexKey = "regex1",
//                                     Language = "{LANGUAGE-CODE}",
//                                     RegexPattern = "{REGEX-PATTERN}"
//                                 }
//                             },
//                             RequiredComponents = new List<string> { "{REQUIRED-COMPONENTS}" }
//                         }
//                     },
//                     Utterances = new List<Utterance>
//                     {
//                         new Utterance
//                         {
//                             Text = "utterance1",
//                             Intent = "intent1",
//                             Language = "{LANGUAGE-CODE}",
//                             Dataset = "{DATASET}",
//                             Entities = new List<EntityInstance>
//                             {
//                                 new EntityInstance
//                                 {
//                                     Category = "ENTITY1",
//                                     Offset = 6,
//                                     Length = 4
//                                 }
//                             }
//                         }
//                     }
//                 }
//             };

//             // Serialize to JSON
//             var jsonString = JsonSerializer.Serialize(conversationModel, new JsonSerializerOptions { WriteIndented = true });

//             // Your HTTP request logic here...
//             // Send jsonString as the request body

//             // Example:
//             // HttpClient httpClient = new HttpClient();
//             // var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
//             // var response = await httpClient.PostAsync("YOUR_URL_HERE", content);
//         }
//     }
// }