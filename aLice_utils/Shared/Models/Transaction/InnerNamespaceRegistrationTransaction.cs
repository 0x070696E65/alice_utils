namespace aLice_utils.Shared.Models.Transaction;

public class InnerNamespaceRegistrationTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string NamespaceId { get; set; } = "";
    public string Duration { get; set; } = "";
    public string ParentId { get; set; } = "";
    public string Id { get; set; } = "";
    public string RegistrationType { get; set; } = "ROOT";
    public string Name { get; set; } = "";
}