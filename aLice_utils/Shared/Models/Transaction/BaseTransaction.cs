using System.ComponentModel;

namespace aLice_utils.Shared.Models.Transaction;

public class BaseTransaction: IBaseTransaction
{
    public TransactionMeta TransactionMeta { get; set; } = new ();
}
