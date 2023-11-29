namespace aLice_utils.Shared.Models.Transaction;

public class VrfKeyLinkTransaction: BaseTransaction
{
    public InnerVrfKeyLinkTransaction InnerTransaction { get; set; } = new();
}