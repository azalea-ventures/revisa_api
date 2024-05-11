// using System.Net.Http;
// using System.Text.Json;
// using Microsoft.Net.Http.Headers;

// public class LanguageSupportService{

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