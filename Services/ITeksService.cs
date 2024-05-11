using revisa_api.Data.teks;

public interface ITeksService
{
    List<TeksItem> GetTeksItems(List<Guid> ids);
    Task GetTEKS(string endpoint);
}
