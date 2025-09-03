namespace EntryLog.Business.Cryptography;

// Clase para configurar las opciones de Argon2
public class Argon2PasswordHashOptions
{
    public int DegreeOfParallelism { get; set; }
    public int MemorySize { get; set; }
    public int Iterations { get; set; }
    public int SaltSize { get; set; }
    public int HashSize { get; set; }
}