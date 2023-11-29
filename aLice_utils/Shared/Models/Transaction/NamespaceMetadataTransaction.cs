namespace aLice_utils.Shared.Models.Transaction;

public class NamespaceMetadataTransaction: BaseTransaction
{
    public InnerNamespaceMetadataTransaction InnerTransaction { get; set; } = new();
}