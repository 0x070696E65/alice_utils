namespace aLice_utils.Shared.Models.Transaction;

public class CosignatureTransaction: BaseTransaction
{
    public InnerCosignatureTransaction InnerTransaction { get; set; } = new();
}