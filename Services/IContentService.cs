public interface IContentService{
    int PostContent(PostContentRequest content);
    GetContentResponse GetContent(int contentId);
}