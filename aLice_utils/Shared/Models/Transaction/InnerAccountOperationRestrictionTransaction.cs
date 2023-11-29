namespace aLice_utils.Shared.Models.Transaction;

public class InnerAccountOperationRestrictionTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public RestrictionFlags RestrictionFlags { get; set; } = new();
    public List<TransactionClass> RestrictionAdditions { get; set; } = new();
    public List<TransactionClass> RestrictionDeletions { get; set; } = new();
}