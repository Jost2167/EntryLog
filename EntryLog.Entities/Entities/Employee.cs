namespace EntryLog.Entities.Entities;

public class Employee
{
    public int Code { get; set; }
    public string FullName { get; set; } = "";
    public string Position { get; set; } = "";
    public int OrganizationId { get; set; }
    public string BranchOffice { get; set; } = "";
    public string TownName { get; set; } = "";
    public string CostCenter { get; set; } = "";
}