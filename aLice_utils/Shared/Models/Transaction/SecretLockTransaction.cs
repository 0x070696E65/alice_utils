namespace aLice_utils.Shared.Models.Transaction;

public class SecretLockTransaction: BaseTransaction
{
    public InnerSecretLockTransaction InnerTransaction { get; set; } = new();
}