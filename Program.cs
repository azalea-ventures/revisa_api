var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IContentService, ContentService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.MapPost("/content", (IContentService contentService, PostContentRequest request) =>
{
    contentService.PostContent(request);
    return Results.Created("/content", request);
}
).WithOpenApi();

app.Run();
