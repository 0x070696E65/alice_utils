namespace aLice_utils.Shared.Models.Transaction;

public class AccountMetadataTransaction: BaseTransaction
{
    public InnerAccountMetadataTransaction InnerTransaction { get; set; } = new();
}