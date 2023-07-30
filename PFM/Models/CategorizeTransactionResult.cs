using PFM.Models;

public class CategorizeTransactionResult
{
    public Transaction Result { get; set; }
    public List<ValidationError> Errors { get; set; }

    public CategorizeTransactionResult()
    {
        Errors = new List<ValidationError>();
    }
}
