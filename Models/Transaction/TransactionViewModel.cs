namespace OnlineCreditSystem.Models.Transaction;

using Data.Models; 

public class TransactionViewModel
{
    public int Id { get; init; }

    public int AmountCredits { get; set; }

    public string? Comment { get; set; }
    
    public string Date { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; }
}
