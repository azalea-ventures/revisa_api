using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using revisa_api.Data.teks;

public class TeksService : ITeksService
{
    private readonly IDbContextFactory<TeksContext> _dbContextFactory;
    private readonly IHttpClientFactory _httpClientFactory;

    public TeksService(
        IDbContextFactory<TeksContext> dbContextFactory,
        IHttpClientFactory httpClientFactory
    )
    {
        _dbContextFactory = dbContextFactory;
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
            try
            {
                using var context = _dbContextFactory.CreateDbContext();

                TeksSubject? subEntity = await context.TeksSubjects.SingleOrDefaultAsync(s =>
                    s.Id.Equals(subject.Id)
                );

                if (subEntity != null)
                {
                    subEntity.Title = subject.Title;
                }
                else
                {
                    await context.TeksSubjects.AddAsync(subject);
                }
                await context.SaveChangesAsync();
                await context.DisposeAsync();
            }
            catch (SqlException e)
            {
                throw;
            }
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

            try
            {
                await Task.WhenAll(
                    teksItemTypes.Select(async it =>
                    {
                        using var context = _dbContextFactory.CreateDbContext();

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
                        await context.DisposeAsync();
                    })
                );
            }
            catch (SqlException e)
            {
                throw;
            }
        });
        tasks.Add(prepItemTypeTask);

        Task prepTekTask = Task.Run(async () =>
        {
            using var context = _dbContextFactory.CreateDbContext();

            Tek? tekEntity = await context.Teks.FindAsync(
                Guid.Parse(response.CFDocument.identifier)
            );
            Tek tek =
                new()
                {
                    Id = Guid.Parse(response.CFDocument.identifier),
                    SubjectId = Guid.Parse(response.CFDocument.subjectURI.First().identifier),
                    Title = response.CFDocument.title,
                    Description = response.CFDocument.description,
                    AdoptionStatus = response.CFDocument.adoptionStatus,
                    EffectiveYear = null,
                    Notes = response.CFDocument.notes,
                    OfficialSourceUrl = response.CFDocument.officialSourceURL,
                    Language = response.CFDocument.language
                };

            if (tekEntity != null)
            {
                tekEntity.SubjectId = Guid.Parse(response.CFDocument.subjectURI.First().identifier);
                tekEntity.Title = response.CFDocument.title;
                tekEntity.Description = response.CFDocument.description;
                tekEntity.AdoptionStatus = response.CFDocument.adoptionStatus;
                tekEntity.EffectiveYear = null;
                tekEntity.Notes = response.CFDocument.notes;
                tekEntity.OfficialSourceUrl = response.CFDocument.officialSourceURL;
                tekEntity.Language = response.CFDocument.language;
            }
            else
            {
                await context.Teks.AddAsync(tek);
            }
            await context.SaveChangesAsync();
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
                TeksItem teksItem = new TeksItem()
                {
                    Id = Guid.Parse(item.identifier),
                    ParentId = null,
                    ListEnumeration = int.TryParse(item.listEnumeration, out listEnum) ? listEnum : 0,
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
                    using var context = _dbContextFactory.CreateDbContext();
                    try
                    {
                        TeksItem? tekItemEntity = await context.TeksItems.FindAsync(item.Id);

                        Guid parentId;

                        bool hasParent = Guid.TryParse(
                            cfAssociations
                                .SingleOrDefault(a => a.originNodeURI.identifier.Equals(item.Id))
                                ?.destinationNodeURI.identifier,
                            out parentId
                        );

                        if (tekItemEntity != null)
                        {
                            tekItemEntity.ParentId = hasParent ? parentId : null;
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
                        await context.DisposeAsync();
                    }
                    catch (SqlException e)
                    {
                        await context.DisposeAsync();
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException e){
                        await context.DisposeAsync();
                    }
                })
            );
        });
        tasks.Add(prepItemsTask);
        
        Console.WriteLine("\n\n\n****RUNNING TEKS DB TASKS****\n\n\n");
        var finishTask = Task.WhenAll([.. tasks]);
        await finishTask;
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
