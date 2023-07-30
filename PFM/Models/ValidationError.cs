namespace PFM.Models
{
    public class ValidationError
    {
        public string tag { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public ValidationError() { }

        public ValidationError(string tag, string error, string message)
        {
            this.error = error;
            this.message = message;
            this.tag = tag;
        }
    }
    
}
