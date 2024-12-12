using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;

public interface IExternalContentService
{
    Task<Object> AnalyzePdfDocument(string blobName, string pages);
    Task<ExternalContentResponse> GetContentBundle(
        string sourceType,
        string[] contentBundle,
        string? module,
        string? topic,
        string? lesson
    );
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

    // TODO: !!Move to Document API!!
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

            var responseStream = await response.Content.ReadAsStreamAsync();
            if (responseStream == null)
            {
                throw new InvalidOperationException("Response content stream is null.");
            }

            var result = await JsonSerializer.DeserializeAsync<Object>(responseStream);
            if (result == null)
            {
                throw new InvalidOperationException("Deserialized object is null.");
            }
            return result;
        }
    }

    public async Task<ExternalContentResponse> GetContentBundle(
        string sourceType,
        string[] contentBundle,
        string? module,
        string? topic,
        string? lesson
    )
    {
        // check source type, if eureka, proceed to GetEurekaContentBundle
        if (sourceType == "eureka")
        {
            return await GetEurkaContentBundle(contentBundle, module, topic, lesson);
        }
        else
        {
            // was not a eureka source type, return new ExternalContentResponse
            // add an error for invalid source type
            var errorResponse = new ErrorResponse
            {
                Message = "Invalid source type",
                Code = "INVALID_SOURCE_TYPE"
            };
            return new ExternalContentResponse { Error = errorResponse };
        }
    }

    private async Task<ExternalContentResponse> GetEurkaContentBundle(
        string[] contentBundle,
        string? module,
        string? topic,
        string? lesson
    )
    {
        // get content bundle from eureka
        return new();
    }
}
