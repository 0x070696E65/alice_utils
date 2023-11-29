namespace aLice_utils.Shared.Models.Transaction;

public class AccountMosaicRestrictionTransaction: BaseTransaction
{
    public InnerAccountMosaicRestrictionTransaction InnerTransaction { get; set; } = new();
}