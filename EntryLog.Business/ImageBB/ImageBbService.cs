using System.Net.Http.Headers;
using System.Text.Json;
using EntryLog.Business.Constants;
using EntryLog.Business.DTOs;
using Microsoft.Extensions.Options;

namespace EntryLog.Business.ImageBB;

public class ImageBbService : ILoadImagesService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ImageBbOptions _options;
    
    public ImageBbService(IHttpClientFactory httpClientFactory, IOptions<ImageBbOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }
    
    public async Task<string> UploadImageAsync(Stream imageStream, string imageName, string extension, string contentType)
    {
        // MultipartFormDataContent para enviar el archivo por POST
        var form = new MultipartFormDataContent();
        
        // StreamContent encapsula el Stream del archivo a enviar
        var imageContent = new StreamContent(imageStream);
        // Asignar el tipo de contenido del archivo
        imageContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        
        // Agregar el contenido del archivo al formulario
        form.Add(imageContent, "image", imageName);
        
        using var client = _httpClientFactory.CreateClient(ApiNames.ImageBb);
        var url = $"?expiration={_options.ExpirationSeconds}&key={_options.ApiKey}";
        var response = await client.PostAsync(url, form);

        if (response.IsSuccessStatusCode)
        {
            // Leer la respuesta como cadena
            var responseContent = await response.Content.ReadAsStringAsync();
            
            // Deserializar la respuesta JSON
            var imageBbResponse = JsonSerializer.Deserialize<ImageBbResponseDTO>(responseContent);

            return imageBbResponse!.Data.DisplayUrl;
        } else
        {
            return "";
            ;
        }
    }
}