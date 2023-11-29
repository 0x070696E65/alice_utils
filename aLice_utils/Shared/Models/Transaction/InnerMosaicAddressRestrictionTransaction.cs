namespace aLice_utils.Shared.Models.Transaction;

public class InnerMosaicAddressRestrictionTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string MosaicId { get; set; } = "";
    public string RestrictionKey { get; set; } = "0";
    public string PreviousRestrictionValue { get; set; } = "0";
    public string NewRestrictionValue { get; set; } = "0";
    public string TargetAddress { get; set; } = "";
}