using Microsoft.EntityFrameworkCore;
using revisa_api.Data.elps;
using revisa_api.Data.language_supports;
using revisa_api.Data.teks;

public class LanguageSupportService : ILanguageSupportService
{
    private readonly IDbContextFactory<LanguageSupportContext> _languageSupportContextFactory;
    private readonly IDbContextFactory<ElpsContext> _elpsContextFactory;

    public LanguageSupportService(
        IDbContextFactory<LanguageSupportContext> languageSupportContextFactory,
        IDbContextFactory<ElpsContext> elpsContextFactory
    )
    {
        _languageSupportContextFactory = languageSupportContextFactory;
        _elpsContextFactory = elpsContextFactory;
    }

    public ElpsSupportResponse GetElpsSupports(string delivery_date)
    {
        using var languageSupportContext = _languageSupportContextFactory.CreateDbContext();

        var lesson_schedule = languageSupportContext.LessonSchedules.FirstOrDefault(s =>
            s.DeliveryDate == DateOnly.Parse(delivery_date)
        );

        if (lesson_schedule == null)
        {
            return new();
        }

        //TODO: this is sloppy, refactor
        var iclos = languageSupportContext
            .Iclos.Where(i => i.LessonScheduleId == lesson_schedule.Id)
            .ToList();
        ElpsSupportResponse response = null;
        iclos.ForEach(iclo =>
        {
            var strategy_objective = languageSupportContext.StrategyObjectives.FirstOrDefault(d =>
                d.Id == iclo.StrategyObjectiveId
            );
            var strategy = languageSupportContext.LearningStrategiesMods.FirstOrDefault(s =>
                s.Id == strategy_objective.StrategyModId
            );
            var domain_objective = languageSupportContext.DomainObjectives.FirstOrDefault(d =>
                d.Id == strategy_objective.DomainObjectiveId
            );
            var teks_item = languageSupportContext.TeksItems.FirstOrDefault(t =>
                t.Id == iclo.TeksItemId
            );
            response = new ElpsSupportResponse
            {
                ElpsStrategy = strategy.Strategy,
                ElpsDomainObjective = $"({domain_objective.Label}) " + domain_objective.Objective,
                ElpsStrategyIconId = strategy.ImageFileId,
                ElpsStrategyId = strategy_objective.StrategyModId,
                Teks = $"({teks_item.HumanCodingScheme}) " + teks_item.FullStatement,
            };
        });

        languageSupportContext.Dispose();
        return response;
    }

    public PostContentResponse GetElpsSupportsByIcloId(int icloId)
    {
        using var languageSupportContext = _languageSupportContextFactory.CreateDbContext();

        var iclo = languageSupportContext.Iclos.FirstOrDefault(i => i.Id == icloId);

        var strategy_objective = languageSupportContext.StrategyObjectives.FirstOrDefault(d =>
            d.Id == iclo.StrategyObjectiveId
        );
        var strategy = languageSupportContext.LearningStrategiesMods.FirstOrDefault(s =>
            s.Id == strategy_objective.StrategyModId
        );
        var domain_objective = languageSupportContext.DomainObjectives.FirstOrDefault(d =>
            d.Id == strategy_objective.DomainObjectiveId
        );
        var teks_item = languageSupportContext.TeksItems.FirstOrDefault(t =>
            t.Id == iclo.TeksItemId
        );

        languageSupportContext.Dispose();
        return new()
        {
            ElpsStrategy = strategy.Strategy,
            ElpsDomainObjective = $"({domain_objective.Label}) " + domain_objective.Objective,
            ElpsStrategyIconId = strategy.ImageFileId,
            ElpsStrategyFileId = strategy.StrategyFileId,
            ElpsStrategyId = strategy_objective.StrategyModId,
            ElpsStrategyLabel = domain_objective.Label,
            Teks = $"({teks_item.HumanCodingScheme}) " + teks_item.FullStatement,
        };
    }

    public LessonSchedule GetLessonSchedule(DateOnly delivery_date)
    {
        using var languageSupportContext = _languageSupportContextFactory.CreateDbContext( );
        var schedules = languageSupportContext.LessonSchedules.Select(s => s).ToList();
        return languageSupportContext.LessonSchedules.FirstOrDefault(s =>
            s.DeliveryDate == delivery_date
        );
    }

    public Iclo GetIclo(
        List<TeksItem> teks,
        LessonSchedule lessonSchedule,
        StrategyObjective strategyObjective
    )
    {
        using var languageSupportContext = _languageSupportContextFactory.CreateDbContext( );
        Iclo iclo =
            new()
            {
                Iclo1 = "",
                TeksItemId = new Guid("c014386e-dc8e-43b0-849e-1dbb33ba4bdc"),
                LessonScheduleId = lessonSchedule.Id,
                StrategyObjectiveId = strategyObjective.Id
            };

        languageSupportContext.Iclos.Add(iclo);

        languageSupportContext.SaveChanges();

        return iclo;
    }
}

//     // private readonly ContentContext _languageSupportContext;
//     private readonly IHttpClientFactory _httpClientFactory;

//     public LanguageSupportService(
//         // IDbContextFactory<TeksContext> dbContextFactory,
//         IHttpClientFactory httpClientFactory
//     )
//     {
//         // _languageSupportContextFactory = dbContextFactory;
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
