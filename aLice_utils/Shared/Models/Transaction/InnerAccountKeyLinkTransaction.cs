namespace aLice_utils.Shared.Models.Transaction;

public class InnerAccountKeyLinkTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string LinkedPublicKey { get; set; } = "";
    public string LinkAction { get; set; } = "LINK";
}