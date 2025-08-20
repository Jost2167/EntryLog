using EntryLog.Entities.Enums;

namespace EntryLog.Entities.Entities;

public class WorkSession
{
    public Guid Id { get;set; }
    public int EmployeeId { get;set; }
    public TimeSpan? TotalWorked => CheckOut != null ? CheckOut.Date - CheckIn.Date : null;
    public SessionStatus Status { get; set; } 
    public Check CheckIn { get; set; } = new();
    public Check? CheckOut { get; set; } 
}