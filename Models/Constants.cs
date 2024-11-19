public enum RevisaStatus
{
    NONE = 0,
    IMPORTED = 1,
    
    PROCESSED = 2,
    EXPORTED = 3,
    ARCHIVED = 4,
    TRANSLATED = 5,
    ERROR = 6
}

public static class RevisaStatusHelper
{
    public static RevisaStatus FromString(string status)
    {
        return status.ToUpper() switch
        {
            "NONE" => RevisaStatus.NONE,
            "IMPORTED" => RevisaStatus.IMPORTED,
            "PROCESSED" => RevisaStatus.PROCESSED,
            "EXPORTED" => RevisaStatus.EXPORTED,
            "ARCHIVED" => RevisaStatus.ARCHIVED,
            "TRANSLATED" => RevisaStatus.TRANSLATED,
            "ERROR" => RevisaStatus.ERROR,
            _ => throw new ArgumentException("Invalid status value")
        };
    }
}


