namespace aLice_utils.Shared.Models.Transaction;

public class SecretProofTransaction: BaseTransaction
{
    public InnerSecretProofTransaction InnerTransaction { get; set; } = new();
}