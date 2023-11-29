namespace aLice_utils.Shared.Models.Transaction;

public class InnerMosaicAddressRestrictionTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string MosaicId { get; set; } = "";
    public string RestrictionKey { get; set; } = "";
    public string PreviousRestrictionValue { get; set; } = "18446744073709551615";
    public string NewRestrictionValue { get; set; } = "1";
    public string TargetAddress { get; set; } = "";
}