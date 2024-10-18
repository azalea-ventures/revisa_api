using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;

public interface IExternalContentService
{
    Task<Object> AnalyzePdfDocument(string blobName, string pages);
}

public class ExternalContentService : IExternalContentService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string? _blobContainerConnectionStr = Environment.GetEnvironmentVariable(
        "SOURCE_FILES_CONTAINER"
    );
    private IConfiguration _configuration;

    public ExternalContentService(IConfiguration configuration, BlobServiceClient blobServiceClient)
    {
        _configuration = configuration;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Object> AnalyzePdfDocument(string pdfUrl, string pages)
    {
        string classifier = "revisa-pdf-mapperv0.4.0";
        string splitOpt = "auto";
        string baseRoute = "documentintelligence/documentClassifiers/";
        // "/modules/EM1_TEKS_G1_M2_TE_SPA.pdf"
        string route =
            $"{baseRoute}{classifier}:analyze?_overload=classifyDocument&api-version=2024-07-31-preview&stringIndexType=textElements&split={splitOpt}&pages={pages}";
        var requestBody = JsonSerializer.Serialize(new { urlSource = pdfUrl });

        using (var client = new HttpClient())
        using (var request = new HttpRequestMessage())
        {
            // Build the request.
            string url = _configuration.GetValue<string>("AZURE_DOCUMENT_INTELLIGENCE_URL") + route;
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add(
                "Ocp-Apim-Subscription-Key",
                _configuration.GetValue<string>("AZURE_DOCUMENT_INTELLIGENCE_KEY")
            );
            request.RequestUri = new Uri(url);
            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

            return await JsonSerializer.DeserializeAsync<Object>(
                await response.Content.ReadAsStreamAsync()
            );
        }
    }
}
