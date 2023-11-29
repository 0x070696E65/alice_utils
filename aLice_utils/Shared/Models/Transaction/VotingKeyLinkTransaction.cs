namespace aLice_utils.Shared.Models.Transaction;

public class VotingKeyLinkTransaction: BaseTransaction
{
    public InnerVotingKeyLinkTransaction InnerTransaction { get; set; } = new();
}