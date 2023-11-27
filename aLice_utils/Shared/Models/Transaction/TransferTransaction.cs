namespace aLice_utils.Shared.Models.Transaction;

public class TransferTransaction: BaseTransaction
{
    public InnerTransferTransaction InnerTransferTransaction { get; set; } = new();
}