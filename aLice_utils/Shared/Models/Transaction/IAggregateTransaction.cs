namespace aLice_utils.Shared.Models.Transaction;

public interface IAggregateTransaction: IBaseTransaction
{
    public string SignerPublicKey { get; set; }
    public List<IInnerTransaction> TransactionsForValidate { get; set; }
    public Dictionary<string, IInnerTransaction> Transactions { get; set; }
    public int CosignatureCount { get; set; }
    public string IsAnnounce { get; set; }
}
