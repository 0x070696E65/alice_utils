namespace aLice_utils.Shared.Models.Transaction;

public class MultisigAccountModificationTransaction: BaseTransaction
{
    public InnerMultisigAccountModificationTransaction InnerTransaction { get; set; } = new();
}