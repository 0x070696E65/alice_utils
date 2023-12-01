namespace aLice_utils.Shared.Models.Transaction;

public class InnerHashLockTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string Duration { get; set; } = "480";
    public string Hash { get; set; } = "";
}