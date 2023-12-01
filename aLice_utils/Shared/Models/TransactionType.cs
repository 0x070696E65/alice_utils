namespace aLice_utils.Shared.Models;

public enum TransactionType
{
    TransferTransaction,
    MosaicDefinitionTransaction,
    MosaicSupplyChangeTransaction,
    MosaicSupplyRevocationTransaction,
    AccountKeyLinkTransaction,
    NodeKeyLinkTransaction,
    VotingKeyLinkTransaction,
    VrfKeyLinkTransaction,
    HashLockTransaction,
    SecretLockTransaction,
    SecretProofTransaction,
    AccountMetadataTransaction,
    MosaicMetadataTransaction,
    NamespaceMetadataTransaction,
    MultisigAccountModificationTransaction,
    AddressAliasTransaction,
    MosaicAliasTransaction,
    NamespaceRegistrationTransaction,
    AccountAddressRestrictionTransaction,
    AccountMosaicRestrictionTransaction,
    AccountOperationRestrictionTransaction,
    MosaicAddressRestrictionTransaction,
    MosaicGlobalRestrictionTransaction,
    AggregateCompleteTransaction,
    AggregateBondedTransaction,
    CosignatureTransaction,
    OfflineRebuildTransaction,
}

public abstract class TransactionTypeExtension
{
    public static List<string> GetTransactionTypeList()
    {
        return Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>().Select(x => x.ToString()).ToList();
    } 
    public static List<string> GetInnerTransactionTypeList()
    {
        var txList = Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>().Select(x => x.ToString()).ToList();
        txList.RemoveAll(item => item.Contains("Aggregate"));
        txList.RemoveAll(item => item.Contains("Cosignature"));
        txList.RemoveAll(item => item.Contains("OfflineRebuildTransaction"));
        return txList;
    } 
}