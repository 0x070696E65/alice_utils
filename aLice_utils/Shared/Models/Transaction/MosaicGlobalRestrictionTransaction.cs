namespace aLice_utils.Shared.Models.Transaction;

public class MosaicGlobalRestrictionTransaction: BaseTransaction
{
    public InnerMosaicGlobalRestrictionTransaction InnerTransaction { get; set; } = new();
}