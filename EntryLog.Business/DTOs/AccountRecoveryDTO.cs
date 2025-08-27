namespace EntryLog.Business.DTOs
{
    public record AccountRecoveryDTO(
        string token,
        string password,
        string passwordConf
    );

    
}