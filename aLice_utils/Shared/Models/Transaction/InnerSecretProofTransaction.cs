namespace aLice_utils.Shared.Models.Transaction;

public class InnerSecretProofTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string RecipientAddress { get; set; } = "";
    public string Secret { get; set; } = "";
    public string Proof { get; set; } = "";
    public string HashAlgorithm { get; set; } = "SHA3_256";
}