namespace aLice_utils.Shared.Models.Transaction;

public class MosaicAddressRestrictionTransaction: BaseTransaction
{
    public InnerMosaicAddressRestrictionTransaction InnerTransaction { get; set; } = new();
}