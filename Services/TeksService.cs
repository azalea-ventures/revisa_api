using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using revisa_api.Data.language_supports;
using revisa_api.Data.teks;

public interface ITeksService
{
    Task GetTEKS(string endpoint);
}
public class TeksService : ITeksService
{
    private readonly IDbContextFactory<TeksContext> _dbContextFactory;
    private readonly LanguageSupportContext _languageSupportContext;
    private readonly IHttpClientFactory _httpClientFactory;

    public TeksService(
        IDbContextFactory<TeksContext> dbContextFactory,
        LanguageSupportContext languageSupportContext,
        IHttpClientFactory httpClientFactory
    )
    {
        _dbContextFactory = dbContextFactory;
        _languageSupportContext = languageSupportContext;
        _httpClientFactory = httpClientFactory;
    }


    public async Task GetTEKS(string endpoint)
    {
        List<Task> tasks = new List<Task>();
        TEKSResponse? response = await OnGet(endpoint);

        Task prepSubjecTask = Task.Run(async () =>
        {
            TeksSubject subject =
                new()
                {
                    Id = Guid.Parse(response.CFDocument.subjectURI.FirstOrDefault().identifier),
                    Title = response.CFDocument.subjectURI.FirstOrDefault().title
                };
            using var context = _dbContextFactory.CreateDbContext();

            try
            {
                TeksSubject? subEntity = await context.TeksSubjects.SingleOrDefaultAsync(s =>
                    s.Id.Equals(subject.Id)
                );

                var tacChapter = new string(
                    response
                        .CFDocument.title.SkipWhile(c => !char.IsDigit(c))
                        .TakeWhile(c => char.IsDigit(c))
                        .ToArray()
                );

                if (subEntity != null)
                {
                    subEntity.Title = subject.Title;
                    subEntity.TacChapter = tacChapter;
                }
                else
                {
                    subject.TacChapter = tacChapter;
                    await context.TeksSubjects.AddAsync(subject);
                }
                await context.SaveChangesAsync();
            }
            catch (Exception e) { }
            await context.DisposeAsync();
        });
        tasks.Add(prepSubjecTask);

        Task prepItemTypeTask = Task.Run(async () =>
        {
            List<TeksItemType> teksItemTypes = response
                .CFDefinitions.CFItemTypes.Select(typ => new TeksItemType
                {
                    Id = Guid.Parse(typ.identifier),
                    Title = typ.title
                })
                .ToList();

            await Task.WhenAll(
                teksItemTypes.Select(async it =>
                {
                    var context = _dbContextFactory.CreateDbContext();

                    try
                    {
                        TeksItemType? entity = await context.TeksItemTypes.FindAsync(it.Id);
                        if (entity != null)
                        {
                            entity.Title = it.Title;
                        }
                        else
                        {
                            await context.TeksItemTypes.AddAsync(it);
                        }

                        await context.SaveChangesAsync();
                    }
                    catch (SqlException e) { }
                    await context.DisposeAsync();
                })
            );
        });
        tasks.Add(prepItemTypeTask);

        Task prepTekTask = Task.Run(async () =>
        {
            var context = _dbContextFactory.CreateDbContext();

            try
            {
                Tek? tekEntity = await context.Teks.FindAsync(
                    Guid.Parse(response.CFDocument.identifier)
                );

                if (tekEntity == null)
                {
                    Tek tek =
                        new()
                        {
                            Id = Guid.Parse(response.CFDocument.identifier),
                            SubjectId = Guid.Parse(
                                response.CFDocument.subjectURI.First().identifier
                            ),
                            Title = response.CFDocument.title,
                            Description = response.CFDocument.description,
                            AdoptionStatus = response.CFDocument.adoptionStatus,
                            EffectiveYear = null,
                            Notes = response.CFDocument.notes,
                            OfficialSourceUrl = response.CFDocument.officialSourceURL,
                            Language = response.CFDocument.language
                        };
                    await context.Teks.AddAsync(tek);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e) { }
            await context.DisposeAsync();
        });
        tasks.Add(prepTekTask);

        Task prepItemsTask = Task.Run(async () =>
        {
            List<CFAssociation> cfAssociations = response.CFAssociations;
            List<CFItem> noID = response.CFItems.ToList();
            List<TeksItem> items = new();

            response.CFItems.ForEach(item =>
            {
                int listEnum = 0;

                var parentNode = cfAssociations
                    .Where(a => a.originNodeURI.identifier.Equals(item.identifier.ToString()))
                    .FirstOrDefault();

                Guid parentId;
                bool hasParent = Guid.TryParse(
                    parentNode?.destinationNodeURI.identifier,
                    out parentId
                );

                TeksItem teksItem = new TeksItem()
                {
                    Id = Guid.Parse(item.identifier),
                    ParentId = hasParent ? parentId : null,
                    ListEnumeration = int.TryParse(item.listEnumeration, out listEnum)
                        ? listEnum
                        : 0,
                    ItemTypeId =
                        item.CFItemTypeURI != null
                            ? Guid.Parse(item.CFItemTypeURI.identifier)
                            : null,
                    HumanCodingScheme = item.humanCodingScheme ?? null,
                    FullStatement = item.fullStatement ?? null,
                    Language = item.language,
                    LastChangeTea = item.lastChangeDateTime,
                    UploadedAt = DateTime.Now
                };
                items.Add(teksItem);
            });

            await Task.WhenAll(
                items.Select(async item =>
                {
                    var context = _dbContextFactory.CreateDbContext();
                    try
                    {
                        TeksItem? tekItemEntity = await context.TeksItems.FindAsync(item.Id);
                        var parentFromDb = await context.TeksItems.FindAsync(item.ParentId);

                        // final check for parent as record in db - if not, we simply need to resubmit the record to add parent id
                        item.ParentId = parentFromDb != null ? parentFromDb.Id : null;

                        if (tekItemEntity != null)
                        {
                            tekItemEntity.ParentId = item.ParentId;
                            tekItemEntity.ListEnumeration = item.ListEnumeration;
                            tekItemEntity.ItemTypeId = item.ItemTypeId;
                            tekItemEntity.HumanCodingScheme = item.HumanCodingScheme;
                            tekItemEntity.FullStatement = item.FullStatement;
                            tekItemEntity.Language = item.Language;
                            tekItemEntity.LastChangeTea = item.LastChangeTea;
                            tekItemEntity.UploadedAt = DateTime.Now;
                        }
                        else
                        {
                            await context.AddAsync(item);
                        }
                        await context.SaveChangesAsync();
                    }
                    catch (SqlException e) { }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException e) { }
                    await context.DisposeAsync();
                })
            );
        });
        tasks.Add(prepItemsTask);

        Console.WriteLine("\n\n\n****RUNNING TEKS DB TASKS****\n\n\n");
        await Task.WhenAll([.. tasks]);
    }

    private async Task<TEKSResponse?> OnGet(string endnpoint)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, endnpoint)
        {
            Headers =
            {
                { HeaderNames.Accept, "application/json" },
                { HeaderNames.UserAgent, "HttpRequestsSample" }
            }
        };

        var httpClient = _httpClientFactory.CreateClient();

        return await httpClient
            .SendAsync(httpRequestMessage)
            .ContinueWith(
                (res) =>
                {
                    return JsonSerializer.Deserialize<TEKSResponse>(
                        res.Result.Content.ReadAsStream()
                    );
                }
            );
    }
}
