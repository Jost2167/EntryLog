using System.Net.Http.Json;
using EntryLog.Business.Constants;
using EntryLog.Business.Interfaces;
using EntryLog.Business.Mailtrap.Models;
using Microsoft.Extensions.Options;

namespace EntryLog.Business.Mailtrap;

internal class MailtrapApiService : IEmailSendService
{
    private readonly MailtrapApiOptions _options;
    private readonly IHttpClientFactory _clientFactory;
    
    public MailtrapApiService(IOptions<MailtrapApiOptions> options, IHttpClientFactory clientFactory)
    {
        _options = options.Value;
        _clientFactory = clientFactory;
    }

    public async Task<bool> SendEmailWithTemplateAsync(string templateName, string to, object? data = null)
    {
        // Crear el cliente HttpClient usando IHttpClientFactory
        using var client = _clientFactory.CreateClient(ApiNames.MailtrapIO);
            
        // Buscar el UUID de la plantilla por su nombre
        string templateUuid = _options.Templates.First(mt=>mt.Name==templateName).Uuid;

        // Construir el cuerpo de la solicitud
        MailtrapRequestBody mailtrapRequestBody = new MailtrapRequestBody()
        {
            From = new From()
            {
                Email = _options.FromEmail,
                Name = _options.FromName
            },
            To =
            [
                new To()
                {
                    Email = to
                }
            ],
            TemplateUuid = templateUuid,
            TemplateVariables = data
        };
        
        var response = await client.PostAsJsonAsync<MailtrapRequestBody>("/api/send", mailtrapRequestBody);

        return response.IsSuccessStatusCode;
    }
}