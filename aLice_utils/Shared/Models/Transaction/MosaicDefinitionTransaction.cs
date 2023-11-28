namespace aLice_utils.Shared.Models.Transaction;

public class MosaicDefinitionTransaction: BaseTransaction
{
    public InnerMosaicDefinitionTransaction InnerTransaction { get; set; } = new();
}