namespace aLice_utils.Shared.Models.Transaction;

public class AggregateCompleteTransaction: BaseTransaction
{
    public List<IInnerTransaction> TransactionsForValidate { get; set; } = new();
    public Dictionary<string, IInnerTransaction> Transactions { get; set; } = new();
    public int CosignatureCount { get; set; } = 0;
    public string IsAnnounce { get; set; } = "true";
}
