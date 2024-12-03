namespace TestCadastroVendas.Domain.Data;

public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    public int TotalCount { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }

    public PagedResponse() { }

    public PagedResponse(IEnumerable<T> data, int totalCount)
    {
        Data = data;
        TotalCount = totalCount;
        Success = true;
        Message = null;
    }
}
