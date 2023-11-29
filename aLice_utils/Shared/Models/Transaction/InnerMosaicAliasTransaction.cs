namespace aLice_utils.Shared.Models.Transaction;

public class InnerMosaicAliasTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string NamespaceId { get; set; } = "";
    public string MosaicId { get; set; } = "";
    public string AliasAction { get; set; } = "LINK";
}