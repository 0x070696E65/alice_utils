using System.Text;
using System.Text.Json.Nodes;
using aLice_utils.Shared.Models.Transaction;
using CatSdk.Facade;
using CatSdk.Symbol;
using CatSdk.Utils;
using Newtonsoft.Json;
using IBaseTransaction = CatSdk.Symbol.IBaseTransaction;
using JsonSerializer = System.Text.Json.JsonSerializer;
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
        { "AggregateCompleteTransaction", () => new AggregateCompleteTransaction() },
        { "AggregateBondedTransaction", () => new AggregateBondedTransaction() },
        { "CosignatureTransaction", () => new CosignatureTransaction() },
        { "OfflineRebuildTransaction", () => new OfflineRebuildTransaction() },
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

    private static readonly Dictionary<Type, Func<aLice_utils.Shared.Models.Transaction.IBaseTransaction, bool, Task<IBaseTransaction>>> TransactionBuilders = new()
    {
        { typeof(TransferTransaction), (t, b) => Task.FromResult(BuildTransferTransaction((TransferTransaction)t, b)) },
        { typeof(MosaicDefinitionTransaction), (t, b) => Task.FromResult(BuildMosaicDefinitionTransaction((MosaicDefinitionTransaction)t, b)) },
        { typeof(MosaicSupplyChangeTransaction), (t, b) => Task.FromResult(BuildMosaicSupplyChangeTransaction((MosaicSupplyChangeTransaction)t, b)) },
        { typeof(MosaicSupplyRevocationTransaction), (t, b) => Task.FromResult(BuildMosaicSupplyRevocationTransaction((MosaicSupplyRevocationTransaction)t, b)) },
        { typeof(AccountKeyLinkTransaction), (t, b) => Task.FromResult(BuildAccountKeyLinkTransaction((AccountKeyLinkTransaction)t, b)) },
        { typeof(NodeKeyLinkTransaction), (t, b) => Task.FromResult(BuildNodeKeyLinkTransaction((NodeKeyLinkTransaction)t, b)) },
        { typeof(VotingKeyLinkTransaction), (t, b) => Task.FromResult(BuildVotingKeyLinkTransaction((VotingKeyLinkTransaction)t, b)) },
        { typeof(VrfKeyLinkTransaction), (t, b) => Task.FromResult(BuildVrfKeyLinkTransaction((VrfKeyLinkTransaction)t, b)) },
        { typeof(HashLockTransaction), (t, b) => Task.FromResult(BuildHashLockTransaction((HashLockTransaction)t, b)) },
        { typeof(SecretLockTransaction), (t, b) => Task.FromResult(BuildSecretLockTransaction((SecretLockTransaction)t, b)) },
        { typeof(SecretProofTransaction), (t, b) => Task.FromResult(BuildSecretProofTransaction((SecretProofTransaction)t, b)) },
        { typeof(AccountMetadataTransaction),  (t, b) => Task.FromResult(BuildAccountMetadataTransaction((AccountMetadataTransaction)t, b)) },
        { typeof(MosaicMetadataTransaction), (t, b) => Task.FromResult(BuildMosaicMetadataTransaction((MosaicMetadataTransaction)t, b)) },
        { typeof(NamespaceMetadataTransaction), (t, b) => Task.FromResult(BuildNamespaceMetadataTransaction((NamespaceMetadataTransaction)t, b)) },
        { typeof(MultisigAccountModificationTransaction), (t, b) => Task.FromResult(BuildMultisigAccountModificationTransaction((MultisigAccountModificationTransaction)t, b)) },
        { typeof(AddressAliasTransaction), (t, b) => Task.FromResult(BuildAddressAliasTransaction((AddressAliasTransaction)t, b)) },
        { typeof(MosaicAliasTransaction), (t, b) => Task.FromResult(BuildMosaicAliasTransaction((MosaicAliasTransaction)t, b)) },
        { typeof(NamespaceRegistrationTransaction), (t, b) => Task.FromResult(BuildNamespaceRegistrationTransaction((NamespaceRegistrationTransaction)t, b)) },
        { typeof(AccountAddressRestrictionTransaction), (t, b) => Task.FromResult(BuildAccountAddressRestrictionTransaction((AccountAddressRestrictionTransaction)t, b)) },
        { typeof(AccountMosaicRestrictionTransaction), (t, b) => Task.FromResult(BuildAccountMosaicRestrictionTransaction((AccountMosaicRestrictionTransaction)t, b)) },
        { typeof(AccountOperationRestrictionTransaction), (t, b) => Task.FromResult(BuildAccountOperationRestrictionTransaction((AccountOperationRestrictionTransaction)t, b)) },
        { typeof(MosaicAddressRestrictionTransaction), (t, b) => Task.FromResult(BuildMosaicAddressRestrictionTransaction((MosaicAddressRestrictionTransaction)t, b)) },
        { typeof(MosaicGlobalRestrictionTransaction), (t, b) => Task.FromResult(BuildMosaicGlobalRestrictionTransaction((MosaicGlobalRestrictionTransaction)t, b)) },
        { typeof(AggregateCompleteTransaction), async (t, b) => await BuildAggregateCompleteTransaction((AggregateCompleteTransaction)t) },
        { typeof(AggregateBondedTransaction), async (t, b) => await BuildAggregateBondedTransaction((AggregateBondedTransaction)t) },
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

    private static async Task RebuildMetadataInfo(BaseTransaction Transaction)
    {
        if(Transaction.GetType() == typeof(AccountMetadataTransaction))
        {
            var transaction = (AccountMetadataTransaction)Transaction;
            var newValue = Converter.Utf8ToBytes(transaction.InnerTransaction.Value);
            if (Converter.IsHexString(transaction.InnerTransaction.ScopedMetadataKey))
            {
                var json = await NodeServices.GetDataFromApi(transaction.InnerTransaction.Node, $"/metadata?scopedMetadataKey={transaction.InnerTransaction.ScopedMetadataKey}&targetAddress={transaction.InnerTransaction.TargetAddress}");
                var s = JsonNode.Parse(json);
                if (s?["data"][0]["metadataEntry"]["value"] != null)
                {
                    var currentValue = Converter.HexToBytes((string)s["data"][0]["metadataEntry"]?["value"]);
                    transaction.InnerTransaction.UlongScopedMetadataKey = ulong.Parse(transaction.InnerTransaction.ScopedMetadataKey, System.Globalization.NumberStyles.HexNumber);
                    transaction.InnerTransaction.ValueBytes = Converter.Xor(currentValue, Converter.Utf8ToBytes(transaction.InnerTransaction.Value));
                    transaction.InnerTransaction.XorValueSize = (ushort)(newValue.Length - currentValue.Length);
                }
                else
                {
                    throw new NullReferenceException("Metadata not found");    
                }
            }
            else
            {
                transaction.InnerTransaction.UlongScopedMetadataKey = IdGenerator.GenerateUlongKey(transaction.InnerTransaction.ScopedMetadataKey);
                transaction.InnerTransaction.ValueBytes = Converter.Utf8ToBytes(transaction.InnerTransaction.Value);
                transaction.InnerTransaction.XorValueSize = (ushort)newValue.Length;
            }   
        }
        if(Transaction.GetType() == typeof(MosaicMetadataTransaction))
        {
            var transaction = (MosaicMetadataTransaction)Transaction;
            var newValue = Converter.Utf8ToBytes(transaction.InnerTransaction.Value);
            if (Converter.IsHexString(transaction.InnerTransaction.ScopedMetadataKey))
            {
                var json = await NodeServices.GetDataFromApi(transaction.InnerTransaction.Node, $"/metadata?scopedMetadataKey={transaction.InnerTransaction.ScopedMetadataKey}&targetId={transaction.InnerTransaction.TargetMosaicId}");
                var s = JsonNode.Parse(json);
                if (s?["data"][0]["metadataEntry"]["value"] != null)
                {
                    var currentValue = Converter.HexToBytes((string)s["data"][0]["metadataEntry"]?["value"]);
                    transaction.InnerTransaction.UlongScopedMetadataKey = ulong.Parse(transaction.InnerTransaction.ScopedMetadataKey, System.Globalization.NumberStyles.HexNumber);
                    transaction.InnerTransaction.ValueBytes = Converter.Xor(currentValue, Converter.Utf8ToBytes(transaction.InnerTransaction.Value));
                    transaction.InnerTransaction.XorValueSize = (ushort)(newValue.Length - currentValue.Length);
                }
                else
                {
                    throw new NullReferenceException("Metadata not found");    
                }
            }
            else
            {
                transaction.InnerTransaction.UlongScopedMetadataKey = IdGenerator.GenerateUlongKey(transaction.InnerTransaction.ScopedMetadataKey);
                transaction.InnerTransaction.ValueBytes = Converter.Utf8ToBytes(transaction.InnerTransaction.Value);
                transaction.InnerTransaction.XorValueSize = (ushort)newValue.Length;
            }
        }
        if(Transaction.GetType() == typeof(NamespaceMetadataTransaction))
        {
            var transaction = (NamespaceMetadataTransaction)Transaction;
            var newValue = Converter.Utf8ToBytes(transaction.InnerTransaction.Value);
            if (Converter.IsHexString(transaction.InnerTransaction.ScopedMetadataKey))
            {
                var json = await NodeServices.GetDataFromApi(transaction.InnerTransaction.Node, $"/metadata?scopedMetadataKey={transaction.InnerTransaction.ScopedMetadataKey}&targetId={transaction.InnerTransaction.TargetNamespaceId}");
                var s = JsonNode.Parse(json);
                if (s?["data"][0]["metadataEntry"]["value"] != null)
                {
                    var currentValue = Converter.HexToBytes((string)s["data"][0]["metadataEntry"]?["value"]);
                    transaction.InnerTransaction.UlongScopedMetadataKey = ulong.Parse(transaction.InnerTransaction.ScopedMetadataKey, System.Globalization.NumberStyles.HexNumber);
                    transaction.InnerTransaction.ValueBytes = Converter.Xor(currentValue, Converter.Utf8ToBytes(transaction.InnerTransaction.Value));
                    transaction.InnerTransaction.XorValueSize = (ushort)(newValue.Length - currentValue.Length);
                }
                else
                {
                    throw new NullReferenceException("Metadata not found");    
                }
            }
            else
            {
                transaction.InnerTransaction.UlongScopedMetadataKey = IdGenerator.GenerateUlongKey(transaction.InnerTransaction.ScopedMetadataKey);
                transaction.InnerTransaction.ValueBytes = Converter.Utf8ToBytes(transaction.InnerTransaction.Value);
                transaction.InnerTransaction.XorValueSize = (ushort)newValue.Length;
            } 
        }
    }

    public static async Task<IBaseTransaction> BuildTransaction(aLice_utils.Shared.Models.Transaction.IBaseTransaction transaction, bool b)
    {
        if (transaction.GetType() == typeof(AccountMetadataTransaction) || transaction.GetType() == typeof(MosaicMetadataTransaction) || transaction.GetType() == typeof(NamespaceMetadataTransaction))
        {
            await RebuildMetadataInfo((BaseTransaction)transaction);
        }
        if (TransactionBuilders.TryGetValue(transaction.GetType(), out var buildFunction))
        {
            return await buildFunction(transaction, b);
        }
        throw new Exception("Transaction type not defined.");
    }
    
    private static IBaseTransaction BuildTransferTransaction(TransferTransaction Transaction, bool isEmbedded)
    {
        var signerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" 
            ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) 
            : new PublicKey();
        var unresolvedAddress = Transaction.InnerTransaction.RecipientAddress != "" 
            ? new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.RecipientAddress)) 
            : new UnresolvedAddress();
        var message = Transaction.InnerTransaction.Message != "" 
            ? Converter.Utf8ToPlainMessage(Transaction.InnerTransaction.Message) 
            : Array.Empty<byte>();
        var mosaics = new List<UnresolvedMosaic>();
        if (Transaction.InnerTransaction.Mosaics is {Count: > 0 })
        {
            foreach (var mosaic in Transaction.InnerTransaction.Mosaics)
            {
                ulong id;
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
        }
        var mosaicsArray = mosaics.ToArray();

        if (isEmbedded)
        {
            return new EmbeddedTransferTransactionV1
            {
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                SignerPublicKey = signerPublicKey,
                RecipientAddress = unresolvedAddress,
                Message = message,
                Mosaics = mosaicsArray
            };
        }
        else
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new TransferTransactionV1
            {
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = signerPublicKey,
                RecipientAddress = unresolvedAddress,
                Message = message,
                Mosaics = mosaicsArray
            };
        }
    }

    private static IBaseTransaction BuildMosaicDefinitionTransaction(MosaicDefinitionTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedMosaicDefinitionTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                Nonce = new MosaicNonce(Transaction.InnerTransaction.Nonce),
                Id = new MosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicID, 16)),
                Duration = new BlockDuration(ulong.Parse(Transaction.InnerTransaction.Duration)),
                Divisibility = byte.Parse(Transaction.InnerTransaction.Divisibility),
                Flags = new MosaicFlags(Converter.CreateMosaicFlags(
                    Transaction.InnerTransaction.SupplyMutable == "true",
                    Transaction.InnerTransaction.Transferable == "true",
                    Transaction.InnerTransaction.Restrictable == "true",
                    Transaction.InnerTransaction.Revokable == "true")),
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new MosaicDefinitionTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                Nonce = new MosaicNonce(Transaction.InnerTransaction.Nonce),
                Id = new MosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicID, 16)),
                Duration = new BlockDuration(ulong.Parse(Transaction.InnerTransaction.Duration)),
                Divisibility = byte.Parse(Transaction.InnerTransaction.Divisibility),
                Flags = new MosaicFlags(Converter.CreateMosaicFlags(
                    Transaction.InnerTransaction.SupplyMutable == "true",
                    Transaction.InnerTransaction.Transferable == "true",
                    Transaction.InnerTransaction.Restrictable == "true",
                    Transaction.InnerTransaction.Revokable == "true")),
            };
        }
    }
    
    private static IBaseTransaction BuildMosaicSupplyChangeTransaction(MosaicSupplyChangeTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedMosaicSupplyChangeTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicID, 16)),
                Action = Transaction.InnerTransaction.Action == "Increase"
                    ? MosaicSupplyChangeAction.INCREASE
                    : MosaicSupplyChangeAction.DECREASE,
                Delta = new Amount(ulong.Parse(Transaction.InnerTransaction.Delta)),
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new MosaicSupplyChangeTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicID, 16)),
                Action = Transaction.InnerTransaction.Action == "Increase"
                    ? MosaicSupplyChangeAction.INCREASE
                    : MosaicSupplyChangeAction.DECREASE,
                Delta = new Amount(ulong.Parse(Transaction.InnerTransaction.Delta)),
            };
        }
    }

    private static IBaseTransaction BuildMosaicSupplyRevocationTransaction(MosaicSupplyRevocationTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedMosaicSupplyRevocationTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                Mosaic = new UnresolvedMosaic()
                {
                    MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.Mosaic.Id, 16)),
                    Amount = new Amount(ulong.Parse(Transaction.InnerTransaction.Mosaic.Amount))
                },
                SourceAddress = new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.SourceAddress)),
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new MosaicSupplyRevocationTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                Mosaic = new UnresolvedMosaic()
                {
                    MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.Mosaic.Id, 16)),
                    Amount = new Amount(ulong.Parse(Transaction.InnerTransaction.Mosaic.Amount))
                },
                SourceAddress = new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.SourceAddress)),
            };   
        }
    }

    private static IBaseTransaction BuildAccountKeyLinkTransaction(AccountKeyLinkTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedAccountKeyLinkTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                LinkedPublicKey = new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.LinkedPublicKey)),
                LinkAction = Transaction.InnerTransaction.LinkAction == "LINK" ? LinkAction.LINK : LinkAction.UNLINK,
            };   
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new AccountKeyLinkTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                LinkedPublicKey = new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.LinkedPublicKey)),
                LinkAction = Transaction.InnerTransaction.LinkAction == "LINK" ? LinkAction.LINK : LinkAction.UNLINK,
            };
        }
    }
    
    private static IBaseTransaction BuildNodeKeyLinkTransaction(NodeKeyLinkTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedNodeKeyLinkTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                LinkedPublicKey = new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.LinkedPublicKey)),
                LinkAction = Transaction.InnerTransaction.LinkAction == "LINK" ? LinkAction.LINK : LinkAction.UNLINK,
            };   
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new NodeKeyLinkTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                LinkedPublicKey = new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.LinkedPublicKey)),
                LinkAction = Transaction.InnerTransaction.LinkAction == "LINK" ? LinkAction.LINK : LinkAction.UNLINK,
            };
        }
    }
    
    private static IBaseTransaction BuildVrfKeyLinkTransaction(VrfKeyLinkTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedVrfKeyLinkTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                LinkedPublicKey = new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.LinkedPublicKey)),
                LinkAction = Transaction.InnerTransaction.LinkAction == "LINK" ? LinkAction.LINK : LinkAction.UNLINK,
            };   
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new VrfKeyLinkTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                LinkedPublicKey = new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.LinkedPublicKey)),
                LinkAction = Transaction.InnerTransaction.LinkAction == "LINK" ? LinkAction.LINK : LinkAction.UNLINK,
            };
        }
    }
    
    private static IBaseTransaction BuildVotingKeyLinkTransaction(VotingKeyLinkTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedVotingKeyLinkTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                LinkedPublicKey = new VotingPublicKey(Converter.HexToBytes(Transaction.InnerTransaction.LinkedPublicKey)),
                LinkAction = Transaction.InnerTransaction.LinkAction == "LINK" ? LinkAction.LINK : LinkAction.UNLINK,
                StartEpoch = new FinalizationEpoch(uint.Parse(Transaction.InnerTransaction.StartEpoch)),
                EndEpoch = new FinalizationEpoch(uint.Parse(Transaction.InnerTransaction.EndEpoch)),
            };   
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new VotingKeyLinkTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != ""
                    ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey))
                    : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet"
                    ? NETWORKTYPE.MAINNET
                    : NETWORKTYPE.TESTNET,
                LinkedPublicKey = new VotingPublicKey(Converter.HexToBytes(Transaction.InnerTransaction.LinkedPublicKey)),
                LinkAction = Transaction.InnerTransaction.LinkAction == "LINK" ? LinkAction.LINK : LinkAction.UNLINK,
                StartEpoch = new FinalizationEpoch(uint.Parse(Transaction.InnerTransaction.StartEpoch)),
                EndEpoch = new FinalizationEpoch(uint.Parse(Transaction.InnerTransaction.EndEpoch)),
            };
        }
    }
    
    private static IBaseTransaction BuildHashLockTransaction(HashLockTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedHashLockTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                Mosaic = new UnresolvedMosaic()
                {
                    MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.TransactionMeta.NetworkType == "MainNet" ? "6BED913FA20223F8" : "72C0212E67A08BCE", 16)),
                    Amount = new Amount(10000000)
                },
                Duration = new BlockDuration(ulong.Parse(Transaction.InnerTransaction.Duration)),
                Hash = new Hash256(Converter.HexToBytes(Transaction.InnerTransaction.Hash))
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new HashLockTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                Mosaic = new UnresolvedMosaic()
                {
                    MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.TransactionMeta.NetworkType == "MainNet" ? "6BED913FA20223F8" : "72C0212E67A08BCE", 16)),
                    Amount = new Amount(10000000)
                },
                Duration = new BlockDuration(ulong.Parse(Transaction.InnerTransaction.Duration)),
                Hash = new Hash256(Converter.HexToBytes(Transaction.InnerTransaction.Hash))
            };   
        }
    }
    
    private static IBaseTransaction BuildSecretLockTransaction(SecretLockTransaction Transaction, bool isEmbedded)
    {
        var hashAlgorithm = Transaction.InnerTransaction.HashAlgorithm switch
        {
            "SHA3_256" => LockHashAlgorithm.SHA3_256,
            "HASH_160" => LockHashAlgorithm.HASH_160,
            "HASH_256" => LockHashAlgorithm.HASH_256,
            _ => LockHashAlgorithm.SHA3_256
        };
        var unresolvedAddress = Transaction.InnerTransaction.RecipientAddress != "" 
            ? new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.RecipientAddress)) 
            : new UnresolvedAddress();
        if (isEmbedded)
        {
            return new EmbeddedSecretLockTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                Mosaic = new UnresolvedMosaic()
                {
                    MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.Mosaic.Id, 16)),
                    Amount = new Amount(ulong.Parse(Transaction.InnerTransaction.Mosaic.Amount))
                },
                RecipientAddress = unresolvedAddress,
                Duration = new BlockDuration(ulong.Parse(Transaction.InnerTransaction.Duration)),
                Secret = new Hash256(Converter.HexToBytes(Transaction.InnerTransaction.Secret)),
                HashAlgorithm = hashAlgorithm,
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new SecretLockTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                Mosaic = new UnresolvedMosaic()
                {
                    MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.Mosaic.Id, 16)),
                    Amount = new Amount(ulong.Parse(Transaction.InnerTransaction.Mosaic.Amount))
                },
                RecipientAddress = unresolvedAddress,
                Duration = new BlockDuration(ulong.Parse(Transaction.InnerTransaction.Duration)),
                Secret = new Hash256(Converter.HexToBytes(Transaction.InnerTransaction.Secret)),
                HashAlgorithm = hashAlgorithm,
            };   
        }
    }

    private static IBaseTransaction BuildSecretProofTransaction(SecretProofTransaction Transaction, bool isEmbedded)
    {
        var hashAlgorithm = Transaction.InnerTransaction.HashAlgorithm switch
        {
            "SHA3_256" => LockHashAlgorithm.SHA3_256,
            "HASH_160" => LockHashAlgorithm.HASH_160,
            "HASH_256" => LockHashAlgorithm.HASH_256,
            _ => LockHashAlgorithm.SHA3_256
        };
        var unresolvedAddress = Transaction.InnerTransaction.RecipientAddress != "" 
            ? new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.RecipientAddress)) 
            : new UnresolvedAddress();
        if (isEmbedded)
        {
            return new EmbeddedSecretProofTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                Secret = new Hash256(Converter.HexToBytes(Transaction.InnerTransaction.Secret)),
                Proof = Converter.HexToBytes(Transaction.InnerTransaction.Proof),
                HashAlgorithm = hashAlgorithm,
                RecipientAddress = unresolvedAddress,
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new SecretProofTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                Secret = new Hash256(Converter.HexToBytes(Transaction.InnerTransaction.Secret)),
                Proof = Converter.HexToBytes(Transaction.InnerTransaction.Proof),
                HashAlgorithm = hashAlgorithm,
                RecipientAddress = unresolvedAddress,
            };   
        }
    }
    
    private static IBaseTransaction BuildAccountMetadataTransaction(AccountMetadataTransaction Transaction, bool isEmbedded)
    {
        var targetAddress = Transaction.InnerTransaction.TargetAddress != "" 
            ? new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.TargetAddress)) 
            : new UnresolvedAddress();
        
        if (isEmbedded)
        {
            return new EmbeddedAccountMetadataTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                TargetAddress = targetAddress,
                ScopedMetadataKey = Transaction.InnerTransaction.UlongScopedMetadataKey,
                Value = Transaction.InnerTransaction.ValueBytes,
                ValueSizeDelta = Transaction.InnerTransaction.XorValueSize,
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new AccountMetadataTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                TargetAddress = targetAddress,
                ScopedMetadataKey = Transaction.InnerTransaction.UlongScopedMetadataKey,
                Value = Transaction.InnerTransaction.ValueBytes,
                ValueSizeDelta = Transaction.InnerTransaction.XorValueSize,
            };   
        }
    }
    
    private static IBaseTransaction BuildMosaicMetadataTransaction(MosaicMetadataTransaction Transaction, bool isEmbedded)
    {
        var targetAddress = Transaction.InnerTransaction.TargetAddress != "" 
            ? new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.TargetAddress)) 
            : new UnresolvedAddress();
        var targetMosaicId = Transaction.InnerTransaction.TargetMosaicId != "" 
            ? new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.TargetMosaicId, 16)) 
            : new UnresolvedMosaicId();

        if (isEmbedded)
        {
            return new EmbeddedMosaicMetadataTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                TargetMosaicId = targetMosaicId,
                TargetAddress = targetAddress,
                ScopedMetadataKey = Transaction.InnerTransaction.UlongScopedMetadataKey,
                Value = Transaction.InnerTransaction.ValueBytes,
                ValueSizeDelta = Transaction.InnerTransaction.XorValueSize,
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new MosaicMetadataTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                TargetMosaicId = targetMosaicId,
                TargetAddress = targetAddress,
                ScopedMetadataKey = Transaction.InnerTransaction.UlongScopedMetadataKey,
                Value = Transaction.InnerTransaction.ValueBytes,
                ValueSizeDelta = Transaction.InnerTransaction.XorValueSize,
            };   
        }
    }
    
    private static IBaseTransaction BuildNamespaceMetadataTransaction(NamespaceMetadataTransaction Transaction, bool isEmbedded)
    {
        var targetAddress = Transaction.InnerTransaction.TargetAddress != "" 
            ? new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.TargetAddress)) 
            : new UnresolvedAddress();
        var targetNamespaceId = Transaction.InnerTransaction.TargetNamespaceId != "" 
            ? new NamespaceId(Convert.ToUInt64(Transaction.InnerTransaction.TargetNamespaceId, 16)) 
            : new NamespaceId();

        if (isEmbedded)
        {
            return new EmbeddedNamespaceMetadataTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                TargetAddress = targetAddress,
                TargetNamespaceId = targetNamespaceId,
                ScopedMetadataKey = Transaction.InnerTransaction.UlongScopedMetadataKey,
                Value = Transaction.InnerTransaction.ValueBytes,
                ValueSizeDelta = Transaction.InnerTransaction.XorValueSize,
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new NamespaceMetadataTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                TargetAddress = targetAddress,
                TargetNamespaceId = targetNamespaceId,
                ScopedMetadataKey = Transaction.InnerTransaction.UlongScopedMetadataKey,
                Value = Transaction.InnerTransaction.ValueBytes,
                ValueSizeDelta = Transaction.InnerTransaction.XorValueSize,
            };   
        }
    }
    
    private static IBaseTransaction BuildMultisigAccountModificationTransaction(MultisigAccountModificationTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedMultisigAccountModificationTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                MinRemovalDelta = byte.Parse(Transaction.InnerTransaction.MinRemovalDelta),
                MinApprovalDelta = byte.Parse(Transaction.InnerTransaction.MinApprovalDelta),
                AddressAdditions = Transaction.InnerTransaction.AddressAdditions.Select(x => new UnresolvedAddress(Converter.StringToAddress(x.Address))).ToArray(),
                AddressDeletions = Transaction.InnerTransaction.AddressDeletions.Select(x => new UnresolvedAddress(Converter.StringToAddress(x.Address))).ToArray(),
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new MultisigAccountModificationTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET, 
                MinRemovalDelta = byte.Parse(Transaction.InnerTransaction.MinRemovalDelta),
                MinApprovalDelta = byte.Parse(Transaction.InnerTransaction.MinApprovalDelta),
                AddressAdditions = Transaction.InnerTransaction.AddressAdditions.Select(x => new UnresolvedAddress(Converter.StringToAddress(x.Address))).ToArray(),
                AddressDeletions = Transaction.InnerTransaction.AddressDeletions.Select(x => new UnresolvedAddress(Converter.StringToAddress(x.Address))).ToArray(),
            };   
        }
    }
    
    private static IBaseTransaction BuildAddressAliasTransaction(AddressAliasTransaction Transaction, bool isEmbedded)
    {
        var address = Transaction.InnerTransaction.Address != "" 
            ? new Address(Converter.StringToAddress(Transaction.InnerTransaction.Address)) 
            : new Address();
        var namespaceId = Transaction.InnerTransaction.NamespaceId != "" 
            ? new NamespaceId(Convert.ToUInt64(Transaction.InnerTransaction.NamespaceId, 16)) 
            : new NamespaceId();
        if (isEmbedded)
        {
            return new EmbeddedAddressAliasTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                NamespaceId = namespaceId,
                Address = address,
                AliasAction = Transaction.InnerTransaction.AliasAction == "LINK" ? AliasAction.LINK : AliasAction.UNLINK,
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new AddressAliasTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                NamespaceId = namespaceId,
                Address = address,
                AliasAction = Transaction.InnerTransaction.AliasAction == "LINK" ? AliasAction.LINK : AliasAction.UNLINK,
            };   
        }
    }
    
    private static IBaseTransaction BuildMosaicAliasTransaction(MosaicAliasTransaction Transaction, bool isEmbedded)
    {
        var mosaicId = Transaction.InnerTransaction.MosaicId != "" 
            ? new MosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicId, 16)) 
            : new MosaicId();
        var namespaceId = Transaction.InnerTransaction.NamespaceId != "" 
            ? new NamespaceId(Convert.ToUInt64(Transaction.InnerTransaction.NamespaceId, 16)) 
            : new NamespaceId();
        if (isEmbedded)
        {
            return new EmbeddedMosaicAliasTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                NamespaceId = namespaceId,
                MosaicId = mosaicId,
                AliasAction = Transaction.InnerTransaction.AliasAction == "LINK" ? AliasAction.LINK : AliasAction.UNLINK,
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new MosaicAliasTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                NamespaceId = namespaceId,
                MosaicId = mosaicId,
                AliasAction = Transaction.InnerTransaction.AliasAction == "LINK" ? AliasAction.LINK : AliasAction.UNLINK,
            };   
        }
    }
    
    private static IBaseTransaction BuildNamespaceRegistrationTransaction(NamespaceRegistrationTransaction Transaction, bool isEmbedded)
    {
        var arr = Transaction.InnerTransaction.Name.Split('.');
        ulong id;
        ulong parentId = 0;
        switch (arr.Length)
        {
            case 1:
                id = IdGenerator.GenerateNamespaceId(Converter.Utf8ToBytes(arr[0]));
                break;
            case 2:
                parentId = IdGenerator.GenerateNamespaceId(Converter.Utf8ToBytes(arr[0]));
                id = IdGenerator.GenerateNamespaceId(Converter.Utf8ToBytes(arr[1]), parentId);
                break;
            case 3:
                var grandParentId = IdGenerator.GenerateNamespaceId(Converter.Utf8ToBytes(arr[0]));
                parentId = IdGenerator.GenerateNamespaceId(Converter.Utf8ToBytes(arr[1]), grandParentId);
                id = IdGenerator.GenerateNamespaceId(Converter.Utf8ToBytes(arr[2]), parentId);
                break;
            default:
                throw new Exception("name is not in the correct format");
        }
        if (isEmbedded)
        {
            var t = new EmbeddedNamespaceRegistrationTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                Duration = new BlockDuration(ulong.Parse(Transaction.InnerTransaction.Duration)),
                Id = new NamespaceId(id),
            };
            if (parentId != 0)
            {
                t.ParentId = new NamespaceId(parentId);
            }

            return t;
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            var t = new NamespaceRegistrationTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                Duration = new BlockDuration(ulong.Parse(Transaction.InnerTransaction.Duration)),
                Id = new NamespaceId(id),
            };
            if (parentId != 0)
            {
                t.ParentId = new NamespaceId(parentId);
            }
            return t;
        }
    }
    
    private static IBaseTransaction BuildAccountAddressRestrictionTransaction(AccountAddressRestrictionTransaction Transaction, bool isEmbedded)
    {
        ushort accountRestrictionFlags = 1;
        if(Transaction.InnerTransaction.OUTGOING == "OUTGOING") accountRestrictionFlags += 16384;
        if(Transaction.InnerTransaction.BLOCK == "BLOCK") accountRestrictionFlags += 32768;

        var addressAdditions = 
            Transaction.InnerTransaction.RestrictionAdditions
                .Select(innerTransactionRestrictionAddition => new UnresolvedAddress(Converter.StringToAddress(innerTransactionRestrictionAddition.Address))).ToArray();

        var addressDeletions = 
            Transaction.InnerTransaction.RestrictionDeletions
                .Select(innerTransactionRestrictionDeletion => new UnresolvedAddress(Converter.StringToAddress(innerTransactionRestrictionDeletion.Address))).ToArray();

        if (isEmbedded)
        {
            return new EmbeddedAccountAddressRestrictionTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                RestrictionFlags = new AccountRestrictionFlags(accountRestrictionFlags),
                RestrictionAdditions = addressAdditions,
                RestrictionDeletions = addressDeletions,
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new AccountAddressRestrictionTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                RestrictionFlags = new AccountRestrictionFlags(accountRestrictionFlags),
                RestrictionAdditions = addressAdditions,
                RestrictionDeletions = addressDeletions,
            };   
        }
    }
    
    private static IBaseTransaction BuildAccountMosaicRestrictionTransaction(AccountMosaicRestrictionTransaction Transaction, bool isEmbedded)
    {
        ushort accountRestrictionFlags = 2;
        if(Transaction.InnerTransaction.OUTGOING == "OUTGOING") accountRestrictionFlags += 16384;
        if(Transaction.InnerTransaction.BLOCK == "BLOCK") accountRestrictionFlags += 32768;

        var mosaicIdAdditions = 
            Transaction.InnerTransaction.RestrictionAdditions
                .Select(innerTransactionRestrictionAddition => new UnresolvedMosaicId(Convert.ToUInt64(innerTransactionRestrictionAddition.Mosaic, 16))).ToArray();
        var mosaicIdDeletions = 
            Transaction.InnerTransaction.RestrictionDeletions
                .Select(innerTransactionRestrictionDeletions => new UnresolvedMosaicId(Convert.ToUInt64(innerTransactionRestrictionDeletions.Mosaic, 16))).ToArray();

        if (isEmbedded)
        {
            return new EmbeddedAccountMosaicRestrictionTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                RestrictionFlags = new AccountRestrictionFlags(accountRestrictionFlags),
                RestrictionAdditions = mosaicIdAdditions,
                RestrictionDeletions = mosaicIdDeletions,
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new AccountMosaicRestrictionTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                RestrictionFlags = new AccountRestrictionFlags(accountRestrictionFlags),
                RestrictionAdditions = mosaicIdAdditions,
                RestrictionDeletions = mosaicIdDeletions,
            };   
        }
    }
    
    private static IBaseTransaction BuildAccountOperationRestrictionTransaction(AccountOperationRestrictionTransaction Transaction, bool isEmbedded)
    {
        ushort accountRestrictionFlags = 4;
        if(Transaction.InnerTransaction.BLOCK == "BLOCK") accountRestrictionFlags += 32768;

        var transactionAdditions = Transaction.InnerTransaction.RestrictionAdditions.Select(innerTransactionRestrictionAddition => ConvertTransactionType(innerTransactionRestrictionAddition.Transaction)).ToArray();
        var transactionDeletions = Transaction.InnerTransaction.RestrictionAdditions.Select(innerTransactionRestrictionAddition => ConvertTransactionType(innerTransactionRestrictionAddition.Transaction)).ToArray();
        
        if (isEmbedded)
        {
            return new EmbeddedAccountOperationRestrictionTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                RestrictionFlags = new AccountRestrictionFlags(accountRestrictionFlags),
                RestrictionAdditions = transactionAdditions,
                RestrictionDeletions = transactionDeletions,
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new AccountOperationRestrictionTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                RestrictionFlags = new AccountRestrictionFlags(accountRestrictionFlags),
                RestrictionAdditions = transactionAdditions,
                RestrictionDeletions = transactionDeletions,
            };   
        }
    }
    
    private static IBaseTransaction BuildMosaicAddressRestrictionTransaction(MosaicAddressRestrictionTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedMosaicAddressRestrictionTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicId, 16)),
                TargetAddress = new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.TargetAddress)),
                RestrictionKey = IdGenerator.GenerateUlongKey(Transaction.InnerTransaction.RestrictionKey),
                PreviousRestrictionValue = ulong.Parse(Transaction.InnerTransaction.PreviousRestrictionValue),
                NewRestrictionValue = ulong.Parse(Transaction.InnerTransaction.NewRestrictionValue),
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new MosaicAddressRestrictionTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicId, 16)),
                TargetAddress = new UnresolvedAddress(Converter.StringToAddress(Transaction.InnerTransaction.TargetAddress)),
                RestrictionKey = IdGenerator.GenerateUlongKey(Transaction.InnerTransaction.RestrictionKey),
                PreviousRestrictionValue = ulong.Parse(Transaction.InnerTransaction.PreviousRestrictionValue),
                NewRestrictionValue = ulong.Parse(Transaction.InnerTransaction.NewRestrictionValue),
            };   
        }
    }
    
    private static IBaseTransaction BuildMosaicGlobalRestrictionTransaction(MosaicGlobalRestrictionTransaction Transaction, bool isEmbedded)
    {
        if (isEmbedded)
        {
            return new EmbeddedMosaicGlobalRestrictionTransactionV1()
            {
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicId, 16)),
                ReferenceMosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.ReferenceMosaicId, 16)),
                RestrictionKey = IdGenerator.GenerateUlongKey(Transaction.InnerTransaction.RestrictionKey),
                PreviousRestrictionValue = ulong.Parse(Transaction.InnerTransaction.PreviousRestrictionValue),
                NewRestrictionValue = ulong.Parse(Transaction.InnerTransaction.NewRestrictionValue),
                PreviousRestrictionType = ConvertMosaicRestrictionType(Transaction.InnerTransaction.PreviousRestrictionType),
                NewRestrictionType = ConvertMosaicRestrictionType(Transaction.InnerTransaction.NewRestrictionType),
            };
        }
        {
            var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
            return new MosaicGlobalRestrictionTransactionV1()
            {
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
                SignerPublicKey = Transaction.InnerTransaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.InnerTransaction.SignerPublicKey)) : new PublicKey(),
                Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET,
                MosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.MosaicId, 16)),
                ReferenceMosaicId = new UnresolvedMosaicId(Convert.ToUInt64(Transaction.InnerTransaction.ReferenceMosaicId, 16)),
                RestrictionKey = IdGenerator.GenerateUlongKey(Transaction.InnerTransaction.RestrictionKey),
                PreviousRestrictionValue = ulong.Parse(Transaction.InnerTransaction.PreviousRestrictionValue),
                NewRestrictionValue = ulong.Parse(Transaction.InnerTransaction.NewRestrictionValue),
                PreviousRestrictionType = ConvertMosaicRestrictionType(Transaction.InnerTransaction.PreviousRestrictionType),
                NewRestrictionType = ConvertMosaicRestrictionType(Transaction.InnerTransaction.NewRestrictionType),
            };   
        }
    }
    
    private static async Task<AggregateCompleteTransactionV2> BuildAggregateCompleteTransaction(AggregateCompleteTransaction Transaction)
    {
        var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
        var aggTx = new AggregateCompleteTransactionV2()
        {
            SignerPublicKey = Transaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.SignerPublicKey)) : new PublicKey(),
            Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
            Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET
        };
        var innerTransactions = (await Task.WhenAll(Transaction.Transactions.Select(t => BuildTransactionFromInner(t.Value)))).ToList();
        foreach (var innerTransaction in innerTransactions)
        {
            innerTransaction.Network = aggTx.Network;
        }
        var merkleHash = SymbolFacade.HashEmbeddedTransactions(innerTransactions.ToArray());
        aggTx.TransactionsHash = merkleHash;
        aggTx.Transactions = innerTransactions.ToArray();

        return aggTx;
    }
    
    private static async Task<AggregateBondedTransactionV2> BuildAggregateBondedTransaction(AggregateBondedTransaction Transaction)
    {
        var facade = new SymbolFacade(Transaction.TransactionMeta.NetworkType == "MainNet" ? Network.MainNet : Network.TestNet);
        var aggTx = new AggregateBondedTransactionV2()
        {
            SignerPublicKey = Transaction.SignerPublicKey != "" ? new PublicKey(Converter.HexToBytes(Transaction.SignerPublicKey)) : new PublicKey(),
            Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddSeconds(ulong.Parse(Transaction.TransactionMeta.Deadline)).Timestamp),
            Network = Transaction.TransactionMeta.NetworkType == "MainNet" ? NETWORKTYPE.MAINNET : NETWORKTYPE.TESTNET
        };
        var innerTransactions = (await Task.WhenAll(Transaction.Transactions.Select(t => BuildTransactionFromInner(t.Value)))).ToList();
        foreach (var innerTransaction in innerTransactions)
        {
            innerTransaction.Network = aggTx.Network;
        }
        var merkleHash = SymbolFacade.HashEmbeddedTransactions(innerTransactions.ToArray());
        aggTx.TransactionsHash = merkleHash;
        aggTx.Transactions = innerTransactions.ToArray();

        return aggTx;
    }

    private static async Task<IBaseTransaction> BuildTransactionFromInner(IInnerTransaction innerTransaction)
    {
        if (innerTransaction.GetType() == typeof(InnerTransferTransaction))
        {
            var tx = new TransferTransaction()
            {
                InnerTransaction = (InnerTransferTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }

        if (innerTransaction.GetType() == typeof(InnerMosaicDefinitionTransaction))
        {
            var tx = new MosaicDefinitionTransaction()
            {
                InnerTransaction = (InnerMosaicDefinitionTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerMosaicSupplyChangeTransaction))
        {
            var tx = new MosaicSupplyChangeTransaction()
            {
                InnerTransaction = (InnerMosaicSupplyChangeTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerMosaicSupplyRevocationTransaction))
        {
            var tx = new MosaicSupplyRevocationTransaction()
            {
                InnerTransaction = (InnerMosaicSupplyRevocationTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerAccountKeyLinkTransaction))
        {
            var tx = new AccountKeyLinkTransaction()
            {
                InnerTransaction = (InnerAccountKeyLinkTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerNodeKeyLinkTransaction))
        {
            var tx = new NodeKeyLinkTransaction()
            {
                InnerTransaction = (InnerNodeKeyLinkTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerVotingKeyLinkTransaction))
        {
            var tx = new VotingKeyLinkTransaction()
            {
                InnerTransaction = (InnerVotingKeyLinkTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerVrfKeyLinkTransaction))
        {
            var tx = new VrfKeyLinkTransaction()
            {
                InnerTransaction = (InnerVrfKeyLinkTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerHashLockTransaction))
        {
            var tx = new HashLockTransaction()
            {
                InnerTransaction = (InnerHashLockTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerSecretLockTransaction))
        {
            var tx = new SecretLockTransaction()
            {
                InnerTransaction = (InnerSecretLockTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerSecretProofTransaction))
        {
            var tx = new SecretProofTransaction()
            {
                InnerTransaction = (InnerSecretProofTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerAccountMetadataTransaction))
        {
            var tx = new AccountMetadataTransaction()
            {
                InnerTransaction = (InnerAccountMetadataTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerMosaicMetadataTransaction))
        {
            var tx = new MosaicMetadataTransaction()
            {
                InnerTransaction = (InnerMosaicMetadataTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerNamespaceMetadataTransaction))
        {
            var tx = new NamespaceMetadataTransaction()
            {
                InnerTransaction = (InnerNamespaceMetadataTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerMultisigAccountModificationTransaction))
        {
            var tx = new MultisigAccountModificationTransaction()
            {
                InnerTransaction = (InnerMultisigAccountModificationTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerAddressAliasTransaction))
        {
            var tx = new AddressAliasTransaction()
            {
                InnerTransaction = (InnerAddressAliasTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerMosaicAliasTransaction))
        {
            var tx = new MosaicAliasTransaction()
            {
                InnerTransaction = (InnerMosaicAliasTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerNamespaceRegistrationTransaction))
        {
            var tx = new NamespaceRegistrationTransaction()
            {
                InnerTransaction = (InnerNamespaceRegistrationTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerAccountAddressRestrictionTransaction))
        {
            var tx = new AccountAddressRestrictionTransaction()
            {
                InnerTransaction = (InnerAccountAddressRestrictionTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerAccountMosaicRestrictionTransaction))
        {
            var tx = new AccountMosaicRestrictionTransaction()
            {
                InnerTransaction = (InnerAccountMosaicRestrictionTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerAccountOperationRestrictionTransaction))
        {
            var tx = new AccountOperationRestrictionTransaction()
            {
                InnerTransaction = (InnerAccountOperationRestrictionTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerMosaicAddressRestrictionTransaction))
        {
            var tx = new MosaicAddressRestrictionTransaction()
            {
                InnerTransaction = (InnerMosaicAddressRestrictionTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        if (innerTransaction.GetType() == typeof(InnerMosaicGlobalRestrictionTransaction))
        {
            var tx = new MosaicGlobalRestrictionTransaction()
            {
                InnerTransaction = (InnerMosaicGlobalRestrictionTransaction) innerTransaction,
            };
            return await BuildTransaction(tx, true);
        }
        
        throw new Exception("Transaction type not found");
    }
    private static (PublicKey signerPublicKey, UnresolvedAddress unresolvedAddress, byte[] message, UnresolvedMosaic[] mosaics) BuildInnerTransferTransaction(InnerTransferTransaction transaction)
    {
        var signerPublicKey = transaction.SignerPublicKey != "" 
            ? new PublicKey(Converter.HexToBytes(transaction.SignerPublicKey)) 
            : new PublicKey();
        var unresolvedAddress = transaction.RecipientAddress != "" 
            ? new UnresolvedAddress(Converter.StringToAddress(transaction.RecipientAddress)) 
            : new UnresolvedAddress();
        var message = transaction.Message != "" 
            ? Converter.Utf8ToPlainMessage(transaction.Message) 
            : Array.Empty<byte>();
        var mosaics = new List<UnresolvedMosaic>();
        if (transaction.Mosaics is {Count: > 0 })
        {
            foreach (var mosaic in transaction.Mosaics)
            {
                ulong id;
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
        }
        var mosaicsArray = mosaics.ToArray();
        return (signerPublicKey, unresolvedAddress, message, mosaicsArray);
    }

    private static TransactionType ConvertTransactionType(string type)
    {
        return type switch
        {
            "TransferTransaction" => TransactionType.TRANSFER,
            "MosaicDefinitionTransaction" => TransactionType.MOSAIC_DEFINITION,
            "MosaicSupplyChangeTransaction" => TransactionType.MOSAIC_SUPPLY_CHANGE,
            "MosaicSupplyRevocationTransaction" => TransactionType.MOSAIC_SUPPLY_REVOCATION,
            "AccountKeyLinkTransaction" => TransactionType.ACCOUNT_KEY_LINK,
            "NodeKeyLinkTransaction" => TransactionType.NODE_KEY_LINK,
            "VotingKeyLinkTransaction" => TransactionType.VOTING_KEY_LINK,
            "VrfKeyLinkTransaction" => TransactionType.VRF_KEY_LINK,
            "HashLockTransaction" => TransactionType.HASH_LOCK,
            "SecretLockTransaction" => TransactionType.SECRET_LOCK,
            "SecretProofTransaction" => TransactionType.SECRET_PROOF,
            "AccountMetadataTransaction" => TransactionType.ACCOUNT_METADATA,
            "MosaicMetadataTransaction" => TransactionType.MOSAIC_METADATA,
            "NamespaceMetadataTransaction" => TransactionType.NAMESPACE_METADATA,
            "MultisigAccountModificationTransaction" => TransactionType.MULTISIG_ACCOUNT_MODIFICATION,
            "AddressAliasTransaction" => TransactionType.ADDRESS_ALIAS,
            "MosaicAliasTransaction" => TransactionType.MOSAIC_ALIAS,
            "NamespaceRegistrationTransaction" => TransactionType.NAMESPACE_REGISTRATION,
            "AccountAddressRestrictionTransaction" => TransactionType.ACCOUNT_ADDRESS_RESTRICTION,
            "AccountMosaicRestrictionTransaction" => TransactionType.ACCOUNT_MOSAIC_RESTRICTION,
            "AccountOperationRestrictionTransaction" => TransactionType.ACCOUNT_OPERATION_RESTRICTION,
            "MosaicAddressRestrictionTransaction" => TransactionType.MOSAIC_ADDRESS_RESTRICTION,
            "MosaicGlobalRestrictionTransaction" => TransactionType.MOSAIC_GLOBAL_RESTRICTION,
            "AggregateCompleteTransaction" => TransactionType.AGGREGATE_COMPLETE,
            "AggregateBondedTransaction" => TransactionType.AGGREGATE_BONDED,
            _ => throw new Exception("Transaction type not found")
        };
    }

    private static MosaicRestrictionType ConvertMosaicRestrictionType(string type)
    {
        return type switch
        {
            "NONE" => MosaicRestrictionType.NONE,
            "EQ" => MosaicRestrictionType.EQ,
            "NE" => MosaicRestrictionType.NE,
            "LT" => MosaicRestrictionType.LT,
            "LE" => MosaicRestrictionType.LE,
            "GT" => MosaicRestrictionType.GT,
            "GE" => MosaicRestrictionType.GE,
            _ => throw new Exception("Mosaic restriction type not found")
        };
    }
}