namespace aLice_utils.Shared.Models.Transaction;

public class InnerMultisigAccountModificationTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string MinRemovalDelta { get; set; } = "0";
    public string MinApprovalDelta { get; set; } = "0";
    public List<string> AddressAdditions { get; set; } = new ();
    public List<string> AddressDeletions { get; set; } = new ();
}