

public class ContentService : IContentService {

    public void PostContent(PostContentRequest content){
        Console.Write(content.ToString());
    }
}