namespace aLice_utils.Shared.Models;

public class TransactionClass
{
    public string Transaction { get; set; }
    public TransactionClass(string transaction)
    {
        Transaction = transaction;
    }
}