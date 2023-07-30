using PFM.Models;

public class PagedSortedListResponse<T>
{
    public PagedSortedList<T> Data { get; set; }
    public List<ValidationError> Errors { get; set; }

    public PagedSortedListResponse(PagedSortedList<T> data, List<ValidationError> errors)
    {
        Data = data;
        Errors = errors;
    }
}
