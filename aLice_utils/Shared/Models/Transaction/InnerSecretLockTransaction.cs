namespace aLice_utils.Shared.Models.Transaction;

public class InnerSecretLockTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string RecipientAddress { get; set; } = "";
    public string Secret { get; set; } = "";
    public Mosaic Mosaic { get; set; } = new ();
    public string Duration { get; set; } = "480";
    public string HashAlgorithm { get; set; } = "SHA3_256";
}