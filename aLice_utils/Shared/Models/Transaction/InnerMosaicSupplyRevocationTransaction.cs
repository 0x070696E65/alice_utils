namespace aLice_utils.Shared.Models.Transaction;

public class InnerMosaicSupplyRevocationTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string SourceAddress { get; set; } = "";
    public Mosaic Mosaic { get; set; } = new ();
}