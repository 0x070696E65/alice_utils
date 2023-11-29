namespace aLice_utils.Shared.Models.Transaction;

public class InnerAccountMosaicRestrictionTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string OUTGOING { get; set; } = "OUTGOING";
    public string BLOCK { get; set; } = "BLOCK";
    public List<MosaicClass> RestrictionAdditions { get; set; } = new();
    public List<MosaicClass> RestrictionDeletions { get; set; } = new();
}