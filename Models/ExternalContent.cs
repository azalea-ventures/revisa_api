public class ExternalContentResponse
{
    // error handling property
    public ErrorResponse Error { get; set; }
}

// need a class for handling error responses
public class ErrorResponse
{
    public string Message { get; set; }
    public string Code { get; set; }
}
