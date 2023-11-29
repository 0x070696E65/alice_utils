namespace aLice_utils.Shared.Models.Transaction;

public class AccountOperationRestrictionTransaction: BaseTransaction
{
    public InnerAccountOperationRestrictionTransaction InnerTransaction { get; set; } = new();
}