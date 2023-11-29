using aLice_utils.Shared.Models.Transaction;
using CatSdk.Facade;
using CatSdk.Symbol;
using CatSdk.Utils;
using NETWORKTYPE = CatSdk.Symbol.NetworkType;

namespace aLice_utils.Client.Services;

public abstract class SymbolBuilderService
{
    private static readonly Dictionary<string, Func<object>> TransactionInstanceBuilders = new()
    {
        { "TransferTransaction", () => new TransferTransaction() },
        { "MosaicDefinitionTransaction", () => new MosaicDefinitionTransaction() },
        { "MosaicSupplyChangeTransaction", () => new MosaicSupplyChangeTransaction() },
        { "MosaicSupplyRevocationTransaction", () => new MosaicSupplyRevocationTransaction() },
        { "AccountKeyLinkTransaction", () => new AccountKeyLinkTransaction() },
        { "NodeKeyLinkTransaction", () => new NodeKeyLinkTransaction() },
        { "VotingKeyLinkTransaction", () => new VotingKeyLinkTransaction() },
        { "VrfKeyLinkTransaction", () => new VrfKeyLinkTransaction() },
        { "HashLockTransaction", () => new HashLockTransaction() },
        { "SecretLockTransaction", () => new SecretLockTransaction() },
        { "SecretProofTransaction", () => new SecretProofTransaction() },
        { "AccountMetadataTransaction", () => new AccountMetadataTransaction() },
        { "MosaicMetadataTransaction", () => new MosaicMetadataTransaction() },
        { "NamespaceMetadataTransaction", () => new NamespaceMetadataTransaction() },
        { "MultisigAccountModificationTransaction", () => new MultisigAccountModificationTransaction() },
        { "AddressAliasTransaction", () => new AddressAliasTransaction() },
        { "MosaicAliasTransaction", () => new MosaicAliasTransaction() },
        { "NamespaceRegistrationTransaction", () => new NamespaceRegistrationTransaction() },
        { "AccountAddressRestrictionTransaction", () => new AccountAddressRestrictionTransaction() },
        { "AccountMosaicRestrictionTransaction", () => new AccountMosaicRestrictionTransaction() },
        { "AccountOperationRestrictionTransaction", () => new AccountOperationRestrictionTransaction() },
        { "MosaicAddressRestrictionTransaction", () => new MosaicAddressRestrictionTransaction() },
        { "MosaicGlobalRestrictionTransaction", () => new MosaicGlobalRestrictionTransaction() },
    };
    
    private static readonly Dictionary<string, Func<object>> InnerTransactionInstanceBuilders = new()
    {
        { "TransferTransaction", () => new InnerTransferTransaction() },
        { "MosaicDefinitionTransaction", () => new InnerMosaicDefinitionTransaction() },
        { "MosaicSupplyChangeTransaction", () => new InnerMosaicSupplyChangeTransaction() },
        { "MosaicSupplyRevocationTransaction", () => new InnerMosaicSupplyRevocationTransaction() },
        { "AccountKeyLinkTransaction", () => new InnerAccountKeyLinkTransaction() },
        { "NodeKeyLinkTransaction", () => new InnerNodeKeyLinkTransaction() },
        { "VotingKeyLinkTransaction", () => new InnerVotingKeyLinkTransaction() },
        { "VrfKeyLinkTransaction", () => new InnerVrfKeyLinkTransaction() },
        { "HashLockTransaction", () => new InnerHashLockTransaction() },
        { "SecretLockTransaction", () => new InnerSecretLockTransaction() },
        { "SecretProofTransaction", () => new InnerSecretProofTransaction() },
        { "AccountMetadataTransaction", () => new InnerAccountMetadataTransaction() },
        { "MosaicMetadataTransaction", () => new InnerMosaicMetadataTransaction() },
        { "NamespaceMetadataTransaction", () => new InnerNamespaceMetadataTransaction() },
        { "MultisigAccountModificationTransaction", () => new InnerMultisigAccountModificationTransaction() },
        { "AddressAliasTransaction", () => new InnerAddressAliasTransaction() },
        { "MosaicAliasTransaction", () => new InnerMosaicAliasTransaction() },
        { "NamespaceRegistrationTransaction", () => new InnerNamespaceRegistrationTransaction() },
        { "AccountAddressRestrictionTransaction", () => new InnerAccountAddressRestrictionTransaction() },
        { "AccountMosaicRestrictionTransaction", () => new InnerAccountMosaicRestrictionTransaction() },
        { "AccountOperationRestrictionTransaction", () => new InnerAccountOperationRestrictionTransaction() },
        { "MosaicAddressRestrictionTransaction", () => new InnerMosaicAddressRestrictionTransaction() },
        { "MosaicGlobalRestrictionTransaction", () => new InnerMosaicGlobalRestrictionTransaction() },
    };

    private static readonly Dictionary<Type, Func<BaseTransaction, ITransaction>> TransactionBuilders = new()
    {
        { typeof(TransferTransaction), t => BuildTransferTransaction((TransferTransaction)t) },
        { typeof(MosaicDefinitionTransaction), t => BuildMosaicDefinitionTransaction((MosaicDefinitionTransaction)t) },
        { typeof(MosaicSupplyChangeTransaction), t => BuildMosaicSupplyChangeTransaction((MosaicSupplyChangeTransaction)t) },
        { typeof(AggregateCompleteTransaction), t => BuildAggregateCompleteTransaction((AggregateCompleteTransaction)t) },
    };

    public static object CreateTransactionInstance(string transactionType, bool isInnerTransaction = false)
    {
        if (isInnerTransaction)
        {
            if (InnerTransactionInstanceBuilders.TryGetValue(transactionType, out var createFunction))
            {
                return createFunction();
            }
        }
        else
        {
            if (TransactionInstanceBuilders.TryGetValue(transactionType, out var createFunction))
            {
                return createFunction();
            }   
        }
        throw new Exception("Transaction type not implemented");
    }

    public static ITransaction BuildTransaction(BaseTransaction transaction)
    {
        if (TransactionBuilders.TryGetValue(transaction.GetType(), out var buildFunction))
        {
            return buildFunction(transaction);
        }

        throw new Exception("Transaction type not defined.");
    }
    
    private static TransferTransactionV1 BuildTransferTransaction(TransferTransaction Transaction)
    {
        var TransferTransactionV1 = new TransferTransactionV1()
        {
            Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
            SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
        };
        if(Transaction.InnerTransaction.RecipientAddress != "") TransferTransactionV1.RecipientAddress = new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.RecipientAddress));
        if (Transaction.InnerTransaction.Message != "") TransferTransactionV1.Message = Converter.Utf8ToPlainMessage(Transaction.InnerTransaction.Message);
        if (Transaction.InnerTransaction.Mosaics is {Count: > 0 })
        {
            var mosaics = new List<UnresolvedMosaic>();
            foreach (var mosaic in Transaction.InnerTransaction.Mosaics)
            {
                ulong id = 0;
                try
                {
                    id = Convert.ToUInt64(mosaic.Id, 16);
                }
                catch (Exception)
                {
                    id = IdGenerator.GenerateNamespaceId(mosaic.Id);
                }
                var UnresolvedMosaic = new UnresolvedMosaic()
                {
                    MosaicId = new UnresolvedMosaicId(id),
                    Amount = new Amount(ulong.Parse(mosaic.Amount))
                };
                mosaics.Add(UnresolvedMosaic);
            }
            TransferTransactionV1.Mosaics = mosaics.ToArray();
        }
        return TransferTransactionV1;
    }

    private static MosaicDefinitionTransactionV1 BuildMosaicDefinitionTransaction(MosaicDefinitionTransaction Transaction)
    {
        return new MosaicDefinitionTransactionV1()
        {
            SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
            Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? CatSdk.Symbol.NetworkType.MAINNET : CatSdk.Symbol.NetworkType.TESTNET,
            Nonce = new MosaicNonce(Transaction.InnerTransaction.Nonce),
            Id = new MosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicID, 16)),
            Duration = new BlockDuration(ulong.Parse(Transaction.InnerTransaction.Duration)),
            Divisibility = byte.Parse(Transaction.InnerTransaction.Divisibility),
            Flags = new MosaicFlags(Converter.CreateMosaicFlags(Transaction.InnerTransaction.SupplyMutable == "true", Transaction.InnerTransaction.Transferable == "true", Transaction.InnerTransaction.Restrictable == "true", Transaction.InnerTransaction.Revokable == "true")),
        };
    }
    
    private static MosaicSupplyChangeTransactionV1 BuildMosaicSupplyChangeTransaction(MosaicSupplyChangeTransaction Transaction)
    {
        return new MosaicSupplyChangeTransactionV1()
        {
            SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
            Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? CatSdk.Symbol.NetworkType.MAINNET : CatSdk.Symbol.NetworkType.TESTNET,
            MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicID, 16)),
            Action = Transaction.InnerTransaction.Action == "Increase" ? MosaicSupplyChangeAction.INCREASE : MosaicSupplyChangeAction.DECREASE,
            Delta = new Amount(ulong.Parse(Transaction.InnerTransaction.Delta)),
        };
    }

    private static AggregateCompleteTransactionV2 BuildAggregateCompleteTransaction(AggregateCompleteTransaction Transaction)
    {
        var aggTx = new AggregateCompleteTransactionV2()
        {
            Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? CatSdk.Symbol.NetworkType.MAINNET : CatSdk.Symbol.NetworkType.TESTNET
        };
        var innerTransactions = new List<IBaseTransaction>();
        
        foreach (var t in Transaction.Transactions)
        {
            if (t.GetType() == typeof(InnerTransferTransaction))
            {
                if(t is not InnerTransferTransaction innerTx) continue;
                var innerTransferTranasction = new EmbeddedTransferTransactionV1
                {
                    Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? CatSdk.Symbol.NetworkType.MAINNET : CatSdk.Symbol.NetworkType.TESTNET
                };
                if(innerTx.RecipientAddress != "") innerTransferTranasction.RecipientAddress = new UnresolvedAddress(Converter.StringToAddress(innerTx.RecipientAddress));
                if (innerTx.Message != "") innerTransferTranasction.Message = Converter.Utf8ToPlainMessage(innerTx.Message);
                if (innerTx.Mosaics is {Count: > 0 })
                {
                    var mosaics = new List<UnresolvedMosaic>();
                    foreach (var mosaic in innerTx.Mosaics)
                    {
                        ulong id = 0;
                        try
                        {
                            id = Convert.ToUInt64(mosaic.Id, 16);
                        }
                        catch (Exception)
                        {
                            id = IdGenerator.GenerateNamespaceId(mosaic.Id);
                        }
                        var UnresolvedMosaic = new UnresolvedMosaic()
                        {
                            MosaicId = new UnresolvedMosaicId(id),
                            Amount = new Amount(ulong.Parse(mosaic.Amount))
                        };
                        mosaics.Add(UnresolvedMosaic);
                    }
                    innerTransferTranasction.Mosaics = mosaics.ToArray();
                }
                innerTransactions.Add(innerTransferTranasction);
            }
        }
        var merkleHash = SymbolFacade.HashEmbeddedTransactions(innerTransactions.ToArray());
        aggTx.TransactionsHash = merkleHash;
        aggTx.Transactions = innerTransactions.ToArray();

        return aggTx;
    }
}