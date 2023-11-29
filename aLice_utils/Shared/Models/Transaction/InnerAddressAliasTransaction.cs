namespace aLice_utils.Shared.Models.Transaction;

public class InnerAddressAliasTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string NamespaceId { get; set; } = "";
    public string Address { get; set; } = "";
    public string AliasAction { get; set; } = "LINK";
}