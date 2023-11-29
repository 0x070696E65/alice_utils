namespace aLice_utils.Shared.Models.Transaction;

public class InnerNamespaceRegistrationTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string Duration { get; set; } = "";
    public string Name { get; set; } = "";
}