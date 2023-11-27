namespace aLice_utils.Shared.Models;

public enum TransactionType
{
    TransferTransaction,
    AccountMetadataTransaction,
    AggregateCompleteTransaction,
}

public abstract class TransactionTypeExtension
{
    public static List<string> GetTransactionTypeList()
    {
        return Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>().Select(x => x.ToString()).ToList();
    } 
}