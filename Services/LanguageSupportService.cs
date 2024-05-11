using System.Net.Http;
using System.Text.Json;
using Microsoft.Net.Http.Headers;

public class LanguageSupportService{

    // private readonly ContentContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;

    public LanguageSupportService(
        // IDbContextFactory<TeksContext> dbContextFactory,
        IHttpClientFactory httpClientFactory
    )
    {
        // _dbContextFactory = dbContextFactory;
        _httpClientFactory = httpClientFactory;
    }


    private async Task OnPost(string endpoint)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint)
        {
            Headers =
            {
                { HeaderNames.Accept, "application/json" },
            }
        };

        var httpClient = _httpClientFactory.CreateClient();

        var res = await httpClient
            .SendAsync(httpRequestMessage)
            .ContinueWith(
                (res) =>
                {
                    return JsonSerializer.Deserialize<TEKSResponse>(
                        res.Result.Content.ReadAsStream()
                    );
                }
            );

        return;
    }
}