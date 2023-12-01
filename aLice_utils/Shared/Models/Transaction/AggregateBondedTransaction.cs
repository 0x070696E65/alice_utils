namespace aLice_utils.Shared.Models.Transaction;

public class AggregateBondedTransaction: BaseTransaction, IAggregateTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public List<IInnerTransaction> TransactionsForValidate { get; set; } = new();
    public Dictionary<string, IInnerTransaction> Transactions { get; set; } = new();
    public int CosignatureCount { get; set; } = 0;
    public string IsAnnounce { get; set; } = "false";
}
