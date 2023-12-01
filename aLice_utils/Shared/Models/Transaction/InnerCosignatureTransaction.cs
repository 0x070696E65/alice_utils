namespace aLice_utils.Shared.Models.Transaction;

public class InnerCosignatureTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string SignedPayload { get; set; } = "";
    public string Signature { get; set; } = "";
    public string Hash { get; set; } = "";
    public string IsAnnounce { get; set; } = "true";
}