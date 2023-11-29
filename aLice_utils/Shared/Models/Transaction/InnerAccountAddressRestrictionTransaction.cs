namespace aLice_utils.Shared.Models.Transaction;

public class InnerAccountAddressRestrictionTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string OUTGOING { get; set; } = "OUTGOING";
    public string BLOCK { get; set; } = "BLOCK";
    public List<AddressClass> RestrictionAdditions { get; set; } = new();
    public List<AddressClass> RestrictionDeletions { get; set; } = new();
}