namespace EntryLog.Data.Pagination;

public class PagedResult<T> where T : class
{
    // Lista de items 
    public List<T> Items { get; set; } = new();
    // Número de página actual
    public int PageNumber { get; set; }
    // Items por página
    public int PageSize { get; set; }
    // Total de items
    public int TotalItems { get; set; }
    // Total de páginas
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PagedResult(List<T> items, int pageNumber, int pageSize, int totalItems)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
    }
}