using revisa_api.Data.teks;

public interface ITeksService
{
    List<TeksItem> GetTeksItems(List<string> ids);
    Task GetTEKS(string endpoint);
}
