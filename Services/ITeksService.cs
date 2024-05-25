using revisa_api.Data.content;
using revisa_api.Data.teks;

public interface ITeksService
{
    List<TeksItem> GetTeksItems(List<string> ids, string grade, Subject subject);
    Task GetTEKS(string endpoint);
}
