namespace aLice_utils.Shared.Models.Transaction;

public class InnerAccountAddressRestrictionTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public RestrictionFlags RestrictionFlags { get; set; } = new();
    public List<AddressClass> RestrictionAdditions { get; set; } = new();
    public List<AddressClass> RestrictionDeletions { get; set; } = new();
}