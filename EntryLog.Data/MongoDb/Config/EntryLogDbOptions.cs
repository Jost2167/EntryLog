namespace EntryLog.Data.MongoDb.Config;

internal sealed class EntryLogDbOptions
{
    //  init es para que unicamente se pueda estrblecer su valor
    // en el moemento de la inicializacion del objeto y no se pueda
    // modificar posteriormente
    
    public string ConnectionUri { get; init; } = "";
    public string DatabaseName { get; init; } = "";
}