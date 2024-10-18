using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using revisa_api.Data.content;
using revisa_api.Data.elps;
using revisa_api.Data.language_supports;
using revisa_api.Data.teks;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Azure services registration
string? blobConnectionString =
    Environment.GetEnvironmentVariable("REVISA_BUCKET")
    ?? builder.Configuration.GetConnectionString("REVISA_BUCKET");

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(blobConnectionString);
});

// Database config setup
string? connectionString =
    Environment.GetEnvironmentVariable("REVISA_DB")
    ?? builder.Configuration.GetConnectionString("REVISA_DB");

Action<DbContextOptionsBuilder> dbConfig = (opt) =>
{
    opt.UseSqlServer(connectionString);
    // opt.EnableSensitiveDataLogging(true);
};

// Database context registration
builder.Services.AddDbContext<ContentContext>(dbConfig);
builder.Services.AddDbContextFactory<LanguageSupportContext>(dbConfig);
builder.Services.AddDbContext<LanguageSupportContext>(dbConfig);
builder.Services.AddPooledDbContextFactory<TeksContext>(dbConfig, 3000);
builder.Services.AddDbContextFactory<ElpsContext>(dbConfig);
builder.Services.AddDbContext<ElpsContext>(dbConfig);

// Service registration
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<ITeksService, TeksService>();
builder.Services.AddScoped<ILanguageSupportService, LanguageSupportService>();
builder.Services.AddScoped<IElpsService, ElpsService>();
builder.Services.AddScoped<ITranslatorService, TranslatorService>();
builder.Services.AddScoped<IExternalContentService, ExternalContentService>();
builder.Services.AddHttpClient();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

/**
send content to post, get back initial elps supports
**/

app.MapPost(
        "/content",
        (PostContentRequest request, IContentService contentService) =>
        {
            int contentId = contentService.PostContent(request);
            return Results.Created("/content", contentId);
        }
    )
    .WithOpenApi();

app.MapGet(
        "/content",
        GetContentResponse (int id, IContentService contentService) =>
        {
            GetContentResponse response = contentService.GetContent(id);
            return response;
        }
    )
    .WithOpenApi();

app.MapPost(
        "/content/info",
        PostContentInfoResponse (PostContentBaseRequest request, IContentService contentService) =>
        {
            PostContentInfoResponse response = contentService.PostContentInfo(request);
            return response;
        }
    )
    .WithOpenApi();

app.MapGet(
        "/content/info",
        (
            [FromQuery(Name = "id")] int? id,
            [FromQuery(Name = "subject")] string? subject,
            IContentService contentService
        ) =>
        {
            if (subject != null && !subject.Equals(""))
            {
                return Results.Ok(contentService.GetContentInfoBySubject(subject));
            }
            else
            {
                return Results.Ok(contentService.GetContentInfo((int)id));
            }
        }
    )
    .WithOpenApi();

app.MapPost(
        "teks",
        async Task (string endpoint, ITeksService teksConsumerService) =>
        {
            await teksConsumerService.GetTEKS(endpoint);
        }
    )
    .WithOpenApi();

app.MapPost(
        "/translate",
        async Task<List<Content>> (
            int contentId,
            ITranslatorService translatorService,
            IContentService contentService
        ) =>
        {
            GetContentResponse contentResponse = contentService.GetContent(contentId);
            return await translatorService.TranslateContent(contentResponse.Content);
        }
    )
    .WithOpenApi();

app.MapPost(
        "/supports/elps",
        (ElpsSupportsRequest request, IElpsService elpsService) =>
        {
            elpsService.SetElpsSupports(request);
        }
    )
    .WithOpenApi();

app.MapGet(
        "/supports/elps",
        ElpsSupportResponse (
            [FromQuery(Name = "deliveryDate")] string deliveryDate,
            [FromQuery(Name = "subject")] string subject,
            [FromQuery(Name = "grade")] string grade,
            ILanguageSupportService languageSupportService
        ) =>
        {
            return languageSupportService.GetElpsSupports(deliveryDate, grade, subject);
        }
    )
    .WithOpenApi();

app.MapGet(
        "/content/external",
        async (
            [FromQuery(Name = "pdfUrl")] string pdfUrl,
            [FromQuery(Name = "pages")] string pages,
            IExternalContentService externalContentService
        ) =>
        {
            return await externalContentService.AnalyzePdfDocument(pdfUrl, pages);
        }
    )
    .WithOpenApi();

// app.MapGet(
//     "/content/source",
//     () =>
//     {
//         PdfDocumentParser.ParsePdfDocument();
//     }
// );

app.Run();
