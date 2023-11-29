namespace aLice_utils.Shared.Models.Transaction;

public class AggregateBondedTransaction: BaseTransaction
{
    public List<IInnerTransaction> Transactions { get; set; } = new();
}
