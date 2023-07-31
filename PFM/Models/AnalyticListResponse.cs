namespace PFM.Models
{
    public class AnalyticListResponse
    {

        public IEnumerable<Analytic> Analytics{ get; set; }
        public List<ValidationError> Errors { get; set; }

        public AnalyticListResponse()
        {
            Errors = new List<ValidationError>();
        }

        public AnalyticListResponse(IEnumerable<Analytic> analytics, List<ValidationError> errors)
        {
            Analytics = analytics;
            Errors = errors;
        }
    }
}
