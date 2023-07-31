namespace PFM.Models
{
    public class AutoCategorizationRule
    {
        public string Title { get; set; }
        public string CatCode { get; set; }
        public string Predicate { get; set; }
        public AutoCategorizationRule()
        {

        }
        public AutoCategorizationRule(string title, string catCode, string predicate)
        {
            Title = title;
            CatCode = catCode;
            Predicate = predicate;
        }
    }
}
