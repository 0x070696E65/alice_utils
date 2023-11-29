namespace aLice_utils.Shared.Models.Transaction;

public class MosaicSupplyRevocationTransaction: BaseTransaction
{
    public InnerMosaicSupplyRevocationTransaction InnerTransaction { get; set; } = new();
}