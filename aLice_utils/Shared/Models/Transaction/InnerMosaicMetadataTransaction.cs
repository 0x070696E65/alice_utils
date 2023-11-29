namespace aLice_utils.Shared.Models.Transaction;

public class InnerMosaicMetadataTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string TargetAddress { get; set; } = "";
    public string TargetMosaicId { get; set; } = "";
    public string ScopedMetadataKey { get; set; } = "";
    public string Value { get; set; } = "";
}