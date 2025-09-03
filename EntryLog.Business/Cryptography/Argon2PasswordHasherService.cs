using System.Security.Cryptography;
using System.Text;
using EntryLog.Business.Interfaces;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace EntryLog.Business.Cryptography;

public class Argon2PasswordHasherService : IPasswordHasherService
{
    private readonly Argon2PasswordHashOptions _options;
    
    public Argon2PasswordHasherService(IOptions<Argon2PasswordHashOptions> options)
    {
        _options = options.Value;
    }
    public string Hash(string password)
    {
        // Generar una sal aleatoria
        var salt = GenerateSalt(_options.SaltSize);
        
        // Hashear la contraseña con la sal
        var hash = HashPassword(password, salt);
        
        // Devolver la sal y el hash concatenados
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string hash)
    {
        // Separar la sal y el hash
        string[] parts = hash.Split(':');
        
        if (parts.Length != 2)
            throw new FormatException("El formato del hash es incorrecto.");
        
        // Extraer y convertir la sal de Base64 a byte[]
        byte[] salt = Convert.FromBase64String(parts[0]);
        // Extraer y convertir el hash de Base64 a byte[]
        byte[] storedHash = Convert.FromBase64String(parts[1]);
        // Hashear la contraseña proporcionada con la misma sal
        byte[] computedHash = HashPassword(password, salt);
        
        // Comparar el hash almacenado con el hash computado
        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }
    
    private byte[] HashPassword(string password, byte[] salt)
    {
        // Configurar las opciones de Argon2
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,                                        // Sal de la contraseña
            DegreeOfParallelism = _options.DegreeOfParallelism, // Número de hilos a usar
            MemorySize = _options.MemorySize,                   // Memoria a usar en KB
            Iterations = _options.Iterations                    // Número de iteraciones
        };
        
        // Generar el hash
        return argon2.GetBytes(_options.HashSize);
    }
    
    private byte[] GenerateSalt(int size)
    {
        // Crear un array de bytes para la sal
        var salt = new byte[size];
        
        // Usar un generador de números aleatorios seguro para llenar la sal
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }
}