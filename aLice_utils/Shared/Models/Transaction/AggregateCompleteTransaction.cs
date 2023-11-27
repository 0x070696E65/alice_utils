namespace aLice_utils.Shared.Models.Transaction;

public class AggregateCompleteTransaction: BaseTransaction
{
    public List<IInnerTransaction> Transactions { get; set; } = new();
}
