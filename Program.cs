using revisa_api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IContentService, ContentService>();
builder.Services.AddDbContext<RevisaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("REVISA_DB"));

});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.MapPost("/content", (PostContentRequest request, IContentService contentService) =>
{
    contentService.PostContent(request);
    return Results.Created("/content", request);
}
).WithOpenApi();

app.MapGet("/content", (int id, IContentService contentService) =>
{
    GetContentResponse response = contentService.GetContent(id);
    return Results.Ok(response);
});

app.Run();
