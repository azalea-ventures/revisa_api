using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            IContentService contentService
        ) =>
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

app.MapGet(
        "/language_supports/iclo",
        ElpsSupportResponse (string delivery_date, ILanguageSupportService languageSupportService) =>
        {
            ElpsSupportResponse response = languageSupportService.GetElpsSupports(delivery_date);
            return response;
        }
    )
    .WithOpenApi();

app.Run();
