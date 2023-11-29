namespace aLice_utils.Shared.Models.Transaction;

public class InnerAccountMosaicRestrictionTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public RestrictionFlags RestrictionFlags { get; set; } = new();
    public List<MosaicClass> RestrictionAdditions { get; set; } = new();
    public List<MosaicClass> RestrictionDeletions { get; set; } = new();
}