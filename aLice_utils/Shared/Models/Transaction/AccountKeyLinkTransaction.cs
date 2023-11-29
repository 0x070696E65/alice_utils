namespace aLice_utils.Shared.Models.Transaction;

public class AccountKeyLinkTransaction: BaseTransaction
{
    public InnerAccountKeyLinkTransaction InnerTransaction { get; set; } = new();
}