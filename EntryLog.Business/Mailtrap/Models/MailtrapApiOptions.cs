namespace EntryLog.Business.Mailtrap.Models;

public class MailtrapApiOptions
{
    public string ApiUrl { get; init; } = ""; 
    public string ApiToken { get; init; } = "";
    public string FromEmail { get; init; } = "";
    public string FromName { get; init; } = "";
    public List<MailtrapTemplate> Templates { get; init; } = [];
}

public class MailtrapTemplate
{
    public string Uuid { get; init; } = "";
    public string Name { get; init; } = "";
}