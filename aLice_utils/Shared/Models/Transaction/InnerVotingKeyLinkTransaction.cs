namespace aLice_utils.Shared.Models.Transaction;

public class InnerVotingKeyLinkTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string LinkedPublicKey { get; set; } = "";
    public string LinkAction { get; set; } = "LINK";
    public string StartEpoch { get; set; } = "0";
    public string EndEpoch { get; set; } = "0";
}