using System.Text;
using System.Text.Json;
public interface ITranslatorService {
    Task<List<Content>> TranslateContent(List<List<Content> >content);
}
public class TranslatorService : ITranslatorService
{
    private IConfiguration configuration;

    public TranslatorService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<List<Content>> TranslateContent(List<List<Content>> content)
    {
        // Input and output languages are defined as parameters.
        string route = "/translate?api-version=3.0&from=en&to=es&includeAlignment=true&category=2abca732-82ea-4257-9236-971859e71efb-custom_translate_math-EDUCATI";
        List<TranslateContent> textRequestList = new();
        // we will keep track of which txt element goes which which element object (on the slide)
        int elementIndex = 0;
        content.ForEach(
            (slide) =>
            {
                textRequestList.AddRange(
                    slide.Select(
                        (s) =>
                        {
                            return new TranslateContent
                            {
                                ObjectId = s.ObjectId,
                                OriginalIndex = elementIndex++,
                                TxtObj = new { Text = s.TextContent }
                            };
                        }
                    )
                );
            }
        );

        var requestBody = JsonSerializer.Serialize(textRequestList.Select(r => r.TxtObj));

        using (var client = new HttpClient())
        using (var request = new HttpRequestMessage())
        {
            // Build the request.
            string url = configuration.GetValue<string>("AZURE_TXT_TRANSLATOR_URL") + route;
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add(
                "Ocp-Apim-Subscription-Key",
                configuration.GetValue<string>("AZURE_TXT_TRANSLATOR_KEY")
            );
            request.RequestUri = new Uri(url);
            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            var translationContentResult = await response.Content.ReadAsStringAsync();
            TranslationResponse[] translatedTexts =
                JsonSerializer.Deserialize<TranslationResponse[]>(translationContentResult);

            List<Content> translatedSlideContent = [];

            if (translatedTexts != null && translatedTexts.Length > 0)
            {
                for (int resInd = 0; resInd < translatedTexts.Length; resInd++)
                {
                    TranslateContent matchedTranslation = textRequestList
                        .Where(r => r.OriginalIndex == resInd)
                        .First();

                    translatedSlideContent.Add(
                        new Content
                        {
                            ObjectId = matchedTranslation.ObjectId,
                            TextContent = translatedTexts[resInd].Translations[0].Text
                        }
                    );
                }
            }
            return translatedSlideContent;
        }
    }

    public async Task<string> GetToken()
    {
        // Input and output languages are defined as parameters.

        using (var client = new HttpClient())
        using (var request = new HttpRequestMessage())
        {
            var key = configuration.GetValue<string>("AZURE_TXT_TRANSLATOR_KEY");
            var region = configuration.GetValue<string>("AZURE_TXT_TRANSLATOR_REGION");
            // Build the request.
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(
                configuration.GetValue<string>("AZURE_TXT_TRANSLATOR_TOKEN_URL")
            );

            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            // location required if you're using a multi-service or regional (not global) resource.
            // request.Headers.Add("Ocp-Apim-Subscription-Region", region);

            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            // Read response as a string.
            return await response.Content.ReadAsStringAsync();
            // Console.WriteLine(result);
        }
    }
}
