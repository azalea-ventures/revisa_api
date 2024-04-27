using Microsoft.EntityFrameworkCore;
using revisa_api.Data.content;
using revisa_api.Data.teks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string connectionString = Environment.GetEnvironmentVariable("REVISA_DB") ?? builder.Configuration.GetConnectionString("REVISA_DB");
Action<DbContextOptionsBuilder> dbConfig = (opt) => {
    opt.UseSqlServer(connectionString);
    opt.EnableSensitiveDataLogging(true);
};
builder.Services.AddDbContext<ContentContext>(dbConfig);
builder.Services.AddPooledDbContextFactory<TeksContext>(dbConfig, 2000);
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<ITeksService, TeksService>();
builder.Services.AddHttpClient();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost(
        "/content",
        (PostContentRequest request, IContentService contentService) =>
        {
            contentService.PostContent(request);
            return Results.Created("/content", request);
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
        "teks",
        async Task (string endpoint, ITeksService teksConsumerService) =>
        {
            await teksConsumerService.GetTEKS(endpoint);
        }
    ).WithOpenApi();

app.Run();
