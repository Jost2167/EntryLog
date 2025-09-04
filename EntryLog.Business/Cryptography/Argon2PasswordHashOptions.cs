namespace EntryLog.Business.Cryptography;

// Clase para configurar las opciones de Argon2
public class Argon2PasswordHashOptions
{
    public int DegreeOfParallelism { get; init; }
    public int MemorySize { get; init; }
    public int Iterations { get; init; }
    public int SaltSize { get; init; }
    public int HashSize { get; init; }
}