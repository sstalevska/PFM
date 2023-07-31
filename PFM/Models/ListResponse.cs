namespace PFM.Models
{
    public class ListResponse<T>
    {
        public List<T> Data { get; set; }
        public List<ValidationError> Errors { get; set; }

        public ListResponse(List<T> data, List<ValidationError> errors)
        {
            Data = data;
            Errors = errors;
        }
    }
}