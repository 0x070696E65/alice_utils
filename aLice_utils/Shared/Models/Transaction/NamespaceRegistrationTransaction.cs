namespace aLice_utils.Shared.Models.Transaction;

public class NamespaceRegistrationTransaction: BaseTransaction
{
    public InnerNamespaceRegistrationTransaction InnerTransaction { get; set; } = new();
}