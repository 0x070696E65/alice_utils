namespace aLice_utils.Shared.Models.Transaction;

public class NodeKeyLinkTransaction: BaseTransaction
{
    public InnerNodeKeyLinkTransaction InnerTransaction { get; set; } = new();
}