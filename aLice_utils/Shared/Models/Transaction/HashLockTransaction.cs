namespace aLice_utils.Shared.Models.Transaction;

public class HashLockTransaction: BaseTransaction
{
    public InnerHashLockTransaction InnerTransaction { get; set; } = new();
}