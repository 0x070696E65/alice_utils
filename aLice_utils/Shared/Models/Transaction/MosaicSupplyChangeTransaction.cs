namespace aLice_utils.Shared.Models.Transaction;

public class MosaicSupplyChangeTransaction: BaseTransaction
{
    public InnerMosaicSupplyChangeTransaction InnerTransaction { get; set; } = new();
}