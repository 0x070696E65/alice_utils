namespace aLice_utils.Shared.Models.Transaction;

public class MosaicAliasTransaction: BaseTransaction
{
    public InnerMosaicAliasTransaction InnerTransaction { get; set; } = new();
}