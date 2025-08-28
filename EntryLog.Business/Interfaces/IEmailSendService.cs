namespace EntryLog.Business.Interfaces;

public interface IEmailSendService
{
    Task<bool> SendEmailWithTemplateAsync(string templateName, string to, object? data = null);
}