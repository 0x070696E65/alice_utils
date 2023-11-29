namespace aLice_utils.Shared.Models.Transaction;

public class MosaicMetadataTransaction: BaseTransaction
{
    public InnerMosaicMetadataTransaction InnerTransaction { get; set; } = new();
}