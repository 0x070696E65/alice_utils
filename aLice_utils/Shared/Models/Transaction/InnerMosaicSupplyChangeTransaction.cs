namespace aLice_utils.Shared.Models.Transaction;

public class InnerMosaicSupplyChangeTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string MosaicID { get; set; } = "";
    public string Action { get; set; } = "Increase";
    public string Delta { get; set; } = "0";
}