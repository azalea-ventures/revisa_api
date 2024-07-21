using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using revisa_api.Data.content;
using revisa_api.Data.elps;
using revisa_api.Data.language_supports;
using revisa_api.Data.teks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// db setup and registration
string connectionString =
    Environment.GetEnvironmentVariable("REVISA_DB")
    ?? builder.Configuration.GetConnectionString("REVISA_DB");
Action<DbContextOptionsBuilder> dbConfig = (opt) =>
{
    opt.UseSqlServer(connectionString);
    // opt.EnableSensitiveDataLogging(true);
};
builder.Services.AddDbContext<ContentContext>(dbConfig);
builder.Services.AddDbContextFactory<LanguageSupportContext>(dbConfig);
builder.Services.AddDbContext<LanguageSupportContext>(dbConfig);
builder.Services.AddPooledDbContextFactory<TeksContext>(dbConfig, 3000);
builder.Services.AddDbContextFactory<ElpsContext>(dbConfig);
builder.Services.AddDbContext<ElpsContext>(dbConfig);

// service registration
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<ITeksService, TeksService>();
builder.Services.AddScoped<ILanguageSupportService, LanguageSupportService>();
builder.Services.AddScoped<IElpsService, ElpsService>();

builder.Services.AddHttpClient();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

/**
send content to post, get back initial elps supports
**/

app.MapPost(
        "/content",
        (
            PostContentRequest request,
            IContentService contentService,
            ILanguageSupportService languageSupportService
        ) =>
        {
            int icloId = contentService.PostContent(request);
            PostContentResponse response = languageSupportService.GetElpsSupportsByIcloId(icloId);
            return Results.Created("/content", response);
        }
    )
    .WithOpenApi();

app.MapGet(
        "/content",
        (int id, IContentService contentService) =>
        {
            GetContentResponse response = contentService.GetContent(id);
            return Results.Ok(response);
        }
    )
    .WithOpenApi();

app.MapPost(
        "/content/info",
        (PostContentBaseRequest request, IContentService contentService) =>
        {
            ContentDetail contentInfo = contentService.PostContentInfo(request);
            return Results.Created(
                "content/info",
                new PostContentInfoResponse() { ContentId = contentInfo.Id }
            );
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

app.MapGet(
        "/language_supports/iclo",
        (string delivery_date, ILanguageSupportService languageSupportService) =>
        {
            ElpsSupportResponse response = languageSupportService.GetElpsSupports(delivery_date);
            return Results.Ok(response);
        }
    )
    .WithOpenApi();

app.Run();
