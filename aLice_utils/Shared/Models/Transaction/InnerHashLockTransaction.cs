namespace aLice_utils.Shared.Models.Transaction;

public class InnerHashLockTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public Mosaic Mosaic { get; set; } = new Mosaic();
    public string Duration { get; set; } = "0";
    public string Hash { get; set; } = "";
}