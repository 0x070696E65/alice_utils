namespace aLice_utils.Shared.Models.Transaction;

public class InnerNamespaceMetadataTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string TargetAddress { get; set; } = "";
    public string TargetNamespaceId { get; set; } = "";
    public string ScopedMetadataKey { get; set; } = "";
    public string Value { get; set; } = "";
}