namespace aLice_utils.Shared.Models.Transaction;

public class InnerAccountOperationRestrictionTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public List<string> RestrictionFlags { get; set; } = new();
    public List<string> RestrictionAdditions { get; set; } = new();
    public List<string> RestrictionDeletions { get; set; } = new();
}