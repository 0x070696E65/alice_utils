namespace aLice_utils.Shared.Models.Transaction;

public class InnerAccountOperationRestrictionTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string BLOCK { get; set; } = "BLOCK";
    public List<TransactionClass> RestrictionAdditions { get; set; } = new();
    public List<TransactionClass> RestrictionDeletions { get; set; } = new();
}