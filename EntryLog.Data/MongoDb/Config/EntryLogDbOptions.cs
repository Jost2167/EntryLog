namespace EntryLog.Data.MongoDb.Config;

internal sealed class EntryLogDbOptions
{
    //  init : can only be set during object initialization
    public string ConnectionString { get; init; } = "";
    public string DatabaseName { get; init; } = "";
}