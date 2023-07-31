namespace PFM.Models
{
    public class RuleErrorListResponse<T>
    {
        public T Rule { get; set; }
        public List<ValidationError> Errors { get; set; }
        public RuleErrorListResponse() { }

        public RuleErrorListResponse(T rule, List<ValidationError> errors)
        {
            Rule = rule;
            Errors = errors;
        }
    }
}