namespace aLice_utils.Shared.Models.Transaction;

public class InnerTransferTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string RecipientAddress { get; set; } = "";
    public List<Mosaic> Mosaics { get; set; } = new();
    public string Message { get; set; } = "";
}