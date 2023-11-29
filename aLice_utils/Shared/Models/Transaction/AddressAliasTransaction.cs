namespace aLice_utils.Shared.Models.Transaction;

public class AddressAliasTransaction: BaseTransaction
{
    public InnerAddressAliasTransaction InnerTransaction { get; set; } = new();
}