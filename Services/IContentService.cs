using revisa_api.Data.content;

public interface IContentService{

    ContentDetail PostContentInfo(PostContentBaseRequest request);
    int PostContent(PostContentRequest content);
    GetContentResponse GetContent(int contentId);
    GetContentBaseResponse GetContentInfo(int contentId);

    List<GetContentBaseResponse> GetContentInfoBySubject(string subject);
}