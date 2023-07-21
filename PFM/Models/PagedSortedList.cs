
using PFM.Models.Enums;

namespace PFM.Models
{
    public class PagedSortedList<T>
    {
        public string? transactionKind { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }


        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public SortOrder SortOrder { get; set; }
        public string SortBy { get; set; }
        public List<T> Items { get; set; }
    }
}
