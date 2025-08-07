namespace EntryLog.Entities.Entities;

public class Position
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public ICollection<Employee> Employees { get; set; } = [];
}