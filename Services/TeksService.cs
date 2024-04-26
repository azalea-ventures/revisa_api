using System.Text.Json;
using Microsoft.Net.Http.Headers;
using revisa_api.Data.teks;

public class TeksService : ITeksService
{
    private readonly TeksContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;

    public TeksService(TeksContext dbContext, IHttpClientFactory httpClientFactory)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
    }

    public async Task GetTEKS(string endpoint)
    {
        using var context = _dbContext;
        using var transaction = context.Database.BeginTransaction();

        Func<TEKSResponse, Task> prepSubject = async (TEKSResponse response) =>
        {
            TeksSubject subject =
                new()
                {
                    Id = Guid.Parse(response.CFDocument.subjectURI.FirstOrDefault().identifier),
                    Title = response.CFDocument.subjectURI.FirstOrDefault().title
                };

            TeksSubject? subEntity = context.TeksSubjects.Find(subject.Id);

            if (subEntity != null)
            {
                subEntity.Title = subject.Title;
                context.TeksSubjects.Update(subject);
            }
            else
            {
                context.TeksSubjects.Add(subject);
            }

            // await context.SaveChangesAsync();

        };

      Func<TEKSResponse, Task> taskItemTypePrep = async (TEKSResponse response) =>
        {
            List<TeksItemType> teksItemTypes = response
                .CFDefinitions.CFItemTypes.Select(typ => new TeksItemType
                {
                    Id = Guid.Parse(typ.identifier),
                    Title = typ.title
                })
                .ToList();

            teksItemTypes.ForEach(it =>
            {
                TeksItemType? entity = context.TeksItemTypes.Find(it.Id);
                if (entity != null)
                {
                    entity.Title = it.Title;
                    context.TeksItemTypes.Update(entity);
                }
                else
                {
                    context.TeksItemTypes.Add(it);
                }
            });

            // await context.SaveChangesAsync();
        };

      Func<TEKSResponse, Task> prepTek = async (TEKSResponse response) =>
        {
            Tek? tekEntity = context.Teks.Find(Guid.Parse(response.CFDocument.identifier));
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
                context.Teks.Update(tekEntity);
            }
            else
            {
                context.Teks.Add(tek);
            }
            // await context.SaveChangesAsync();
        };

      Func<TEKSResponse, Task> prepItems = async (TEKSResponse response) =>
        {
            List<CFAssociation> cfAssociations = response.CFAssociations;
            List<CFItem> noID = response.CFItems.ToList();
            List<TeksItem> items = new();

            response.CFItems.ForEach(item =>
            {
                try
                {
                    TeksItem teksItem = new TeksItem()
                    {
                        Id = Guid.Parse(item.identifier),
                        ParentId = null,
                        ListEnumeration = item.listEnumeration ?? null,
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
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine("", e.Message);
                }
            });

            items.ForEach(item =>
            {
                TeksItem? tekItemEntity = context.TeksItems.Find(item.Id);

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
                    context.TeksItems.Update(tekItemEntity);
                }
                else
                {
                    context.Add(item);
                }
            });

            // await context.SaveChangesAsync();
        };

        await OnGet(endpoint)
            .ContinueWith(async tr =>
            {
                var result = tr.Result;
                await prepSubject(result);
                await taskItemTypePrep(result);
                await prepTek(result);
                await prepItems(result);
            })
            .ContinueWith(async (t) => await transaction.CommitAsync());
    }

    private async Task<TEKSResponse> OnGet(string endnpoint)
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

        Func<HttpResponseMessage, TEKSResponse> getTeksResponse = (
            HttpResponseMessage responseTask
        ) =>
        {
            return JsonSerializer.Deserialize<TEKSResponse>(responseTask.Content.ReadAsStream());
        };

        return await httpClient
            .SendAsync(httpRequestMessage)
            .ContinueWith(httpRes => getTeksResponse(httpRes.Result));
    }
}
