namespace aLice_utils.Shared.Models.Transaction;

public class InnerNamespaceMetadataTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string TargetAddress { get; set; } = "";
    public string TargetNamespaceId { get; set; } = "";
    public string ScopedMetadataKey { get; set; } = "";
    public string ValueSizeDelta { get; set; } = "0";
    public string Value { get; set; } = "";
}