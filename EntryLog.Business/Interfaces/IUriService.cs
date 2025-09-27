namespace EntryLog.Business.Inraestructure;

// Servicio para obtener información de la URI y del dispositivo
public interface IUriService
{
    public string ApplicationUrl { get; }
    public string UserAgent { get;} 
    public string Platform { get;}
    public string RemoteIpAddress { get;}
}