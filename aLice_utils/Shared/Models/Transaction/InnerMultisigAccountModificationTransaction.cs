namespace aLice_utils.Shared.Models.Transaction;

public class InnerMultisigAccountModificationTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string MinRemovalDelta { get; set; } = "0";
    public string MinApprovalDelta { get; set; } = "0";
    public List<AddressClass> AddressAdditions { get; set; } = new ();
    public List<AddressClass> AddressDeletions { get; set; } = new ();
}