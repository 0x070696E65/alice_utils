using System.ComponentModel;

namespace aLice_utils.Shared.Models.Transaction;

public class BaseTransaction
{
    public TransactionMeta TransactionMeta { get; set; } = new ();
}