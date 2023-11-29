namespace aLice_utils.Shared.Models.Transaction;

public class AccountAddressRestrictionTransaction: BaseTransaction
{
    public InnerAccountAddressRestrictionTransaction InnerTransaction { get; set; } = new();
}