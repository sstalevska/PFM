namespace PFM.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string Content { get; set; }

        public ApiException(int statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}
