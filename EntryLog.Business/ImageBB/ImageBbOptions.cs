namespace EntryLog.Business.ImageBB;

public record ImageBbOptions(
    string ApiUrl,
    string ApiToken,
    int ExpirationSeconds
    );