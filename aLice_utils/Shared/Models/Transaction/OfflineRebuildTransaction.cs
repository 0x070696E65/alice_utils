namespace aLice_utils.Shared.Models.Transaction;

public class OfflineRebuildTransaction: BaseTransaction
{
    public string SignedPayload { get; set; } = "";
    public string Hash { get; set; } = "";
    public List<InnerCosignatureTransaction> Transactions { get; set; } = new();
}