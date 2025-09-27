using EntryLog.Business.Inraestructure;
using Microsoft.AspNetCore.Http;

namespace EntryLog.Business.Infraestructure;

public class UriService : IUriService
{
    public string ApplicationUrl { get; private set; }
    public string UserAgent { get; private set; }
    public string Platform { get; private set; }
    public string RemoteIpAddress { get; private set; }

    // IHttpContextAccessor permite acceder al contexto HTTP actual
    public UriService(IHttpContextAccessor httpContextAccessor)
    {
        // Obtener el contexto HTTP actual
        var request = httpContextAccessor.HttpContext?.Request 
                      ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        
        // Construir la URL de la aplicacion
        ApplicationUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
        
        // Obtener la direccion ip remota de la solicitud
        RemoteIpAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        
        // Obtener el User-Agent de la solicitud (navegador)
        UserAgent = request.Headers["User-Agent"].ToString() ?? "Unknown";
        
        // Obtener la plataforma del dispositivo (Windows, Mac, Linux, etc.)
        Platform = request.Headers["sec-ch-ua-platform"].ToString() ?? "Unknown";
    }
}