using System.Security.Cryptography;
using System.Text;
using EntryLog.Business.Interfaces;
using Microsoft.Extensions.Options;

namespace EntryLog.Business.Cryptography;

// Implementación del servicio de encriptación asimétrica utilizando RSA (Antiguo)
public class RSAAsymmetricEncryptionService: IEncryptionService
{
    // Instancia de EncryptionKeyValues para acceder a las claves de encriptación
    private readonly EncryptionKeyValues _keys;
    private readonly RSACryptoServiceProvider _csp = new RSACryptoServiceProvider((int)KeySize.SIZE_2048);
    
    // Patron opcion. IOptions<> permite identificar que es un objeto de configuracion inyectada desde appsettings.json
    public RSAAsymmetricEncryptionService(IOptions<EncryptionKeyValues> options)
    {
        _keys = options.Value;
    }
    
    public string Encrypt(string plainText)
    {
        try
        {
            // Cargar la clave pública para encriptar
            _csp.FromXmlString(_keys.PublicKey);
            byte[] data = Encoding.Unicode.GetBytes(plainText);
            byte[] cypher = _csp.Encrypt(data, true);
            return Convert.ToBase64String(cypher);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            // Evitar que la clave se guarde en el contenedor de claves
            _csp.PersistKeyInCsp = false;
        }
    }

    public string Decrypt(string cypherText)
    {
        try
        {
            _csp.FromXmlString(_keys.PrivateKey);
            byte[] data = Convert.FromBase64String(cypherText);
            byte[] plain = _csp.Decrypt(data, true);
            return Encoding.Unicode.GetString(plain);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            _csp.PersistKeyInCsp = false;
        }
    }
}

public enum KeySize
{
    SIZE_512 = 512,
    SIZE_1024 = 1024,
    SIZE_2048 = 2048,
    SIZE_952 = 952,
    SIZE_1369 = 1369
}