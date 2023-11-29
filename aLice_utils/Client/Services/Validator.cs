using aLice_utils.Client.Extensions;
using aLice_utils.Shared.Models;
using aLice_utils.Shared.Models.Transaction;
using FluentValidation;
    
namespace aLice_utils.Client.Services;

public abstract class Validator
{
    private class TransactionMetaValidator : AbstractValidator<TransactionMeta>
    {
        public TransactionMetaValidator()
        {
            RuleFor(meta => meta.Deadline).NotEmpty().IsOver(10).IsUnder(7200);
            RuleFor(meta => meta.Node).NotEmpty().IsNodeUrl();
        }
    }
    
    private class TransferTransactionValidator : AbstractValidator<InnerTransferTransaction>
    {
        public TransferTransactionValidator()
        {
            RuleFor(tranasction => tranasction.Message).IsUnderXByte(1023);
            RuleFor(tranasction => tranasction.RecipientAddress).NotEmpty().IsSymbolAddress();
            RuleForEach(transaction => transaction.Mosaics).SetValidator(new MosaicValidator());
        }
    }
    
    private class MosaicDefinitionTransactionValidator : AbstractValidator<InnerMosaicDefinitionTransaction>
    {
        public MosaicDefinitionTransactionValidator()
        {
            RuleFor(tranasction => tranasction.SignerPublicKey).NotEmpty().IsHex().IsXCharacters(64);
            RuleFor(tranasction => tranasction.MosaicID);
            RuleFor(tranasction => tranasction.Duration).NotEmpty().IsOver(0);
            RuleFor(tranasction => tranasction.Divisibility).NotEmpty().IsOver(0).IsUnder(6);
        }
    }
    
    private class MosaicSupplyChangeTransactionValidator : AbstractValidator<InnerMosaicSupplyChangeTransaction>
    {
        public MosaicSupplyChangeTransactionValidator()
        {
            RuleFor(tranasction => tranasction.MosaicID).NotEmpty().IsHex().IsXCharacters(16);
            RuleFor(tranasction => tranasction.Action).NotEmpty();
            RuleFor(tranasction => tranasction.Delta).NotEmpty().IsOver(0);
        }
    }
    
    private class MosaicSupplyRevocationTransactionValidator : AbstractValidator<InnerMosaicSupplyRevocationTransaction>
    {
        public MosaicSupplyRevocationTransactionValidator()
        {
            RuleFor(tranasction => tranasction.SourceAddress).IsSymbolAddress();
            RuleFor(tranasction => tranasction.Mosaic).SetValidator(new MosaicValidator());
        }
    }
    
    private class AccountKeyLinkTransactionValidator : AbstractValidator<InnerAccountKeyLinkTransaction>
    {
        public AccountKeyLinkTransactionValidator()
        {
        }
    }
    
    private class NodeKeyLinkTransactionValidator : AbstractValidator<InnerNodeKeyLinkTransaction>
    {
        public NodeKeyLinkTransactionValidator()
        {
        }
    }
    
    private class VotingKeyLinkTransactionValidator : AbstractValidator<InnerVotingKeyLinkTransaction>
    {
        public VotingKeyLinkTransactionValidator()
        {
        }
    }
    
    private class VrfKeyLinkTransactionValidator : AbstractValidator<InnerVrfKeyLinkTransaction>
    {
        public VrfKeyLinkTransactionValidator()
        {
        }
    }
    
    private class HashLockTransactionValidator : AbstractValidator<InnerHashLockTransaction>
    {
        public HashLockTransactionValidator()
        {
        }
    }
    
    private class SecretLockTransactionValidator : AbstractValidator<InnerSecretLockTransaction>
    {
        public SecretLockTransactionValidator()
        {
        }
    }
    
    private class SecretProofTransactionValidator : AbstractValidator<InnerSecretProofTransaction>
    {
        public SecretProofTransactionValidator()
        {
        }
    }
    
    private class AccountMetadataTransactionValidator : AbstractValidator<InnerAccountMetadataTransaction>
    {
        public AccountMetadataTransactionValidator()
        {
        }
    }
    
    private class MosaicMetadataTransactionValidator : AbstractValidator<InnerMosaicMetadataTransaction>
    {
        public MosaicMetadataTransactionValidator()
        {
        }
    }
    
    private class NamespaceMetadataTransactionValidator : AbstractValidator<InnerNamespaceMetadataTransaction>
    {
        public NamespaceMetadataTransactionValidator()
        {
        }
    }
    
    private class MultisigAccountModificationTransactionValidator : AbstractValidator<InnerMultisigAccountModificationTransaction>
    {
        public MultisigAccountModificationTransactionValidator()
        {
        }
    }
    
    private class AddressAliasTransactionValidator : AbstractValidator<InnerAddressAliasTransaction>
    {
        public AddressAliasTransactionValidator()
        {
        }
    }
    
    private class MosaicAliasTransactionValidator : AbstractValidator<InnerMosaicAliasTransaction>
    {
        public MosaicAliasTransactionValidator()
        {
        }
    }
    
    private class NamespaceRegistrationTransactionValidator : AbstractValidator<InnerNamespaceRegistrationTransaction>
    {
        public NamespaceRegistrationTransactionValidator()
        {
        }
    }
    
    private class AccountAddressRestrictionTransactionValidator : AbstractValidator<InnerAccountAddressRestrictionTransaction>
    {
        public AccountAddressRestrictionTransactionValidator()
        {
        }
    }
    
    private class AccountMosaicRestrictionTransactionValidator : AbstractValidator<InnerAccountMosaicRestrictionTransaction>
    {
        public AccountMosaicRestrictionTransactionValidator()
        {
        }
    }
    
    private class AccountOperationRestrictionTransactionValidator : AbstractValidator<InnerAccountOperationRestrictionTransaction>
    {
        public AccountOperationRestrictionTransactionValidator()
        {
        }
    }
    
    private class MosaicAddressRestrictionTransactionValidator : AbstractValidator<InnerMosaicAddressRestrictionTransaction>
    {
        public MosaicAddressRestrictionTransactionValidator()
        {
        }
    }
    
    private class MosaicGlobalRestrictionTransactionValidator : AbstractValidator<InnerMosaicGlobalRestrictionTransaction>
    {
        public MosaicGlobalRestrictionTransactionValidator()
        {
        }
    }
    
    public class TransactionValidator : AbstractValidator<BaseTransaction>
    {
        public TransactionValidator()
        {
            try
            {
                RuleFor(baseTransaction => baseTransaction.TransactionMeta)
                    .SetValidator(new TransactionMetaValidator());
                When(transaction => transaction is AggregateCompleteTransaction,
                    () =>
                    {
                        RuleFor(transaction => (AggregateCompleteTransaction) transaction)
                            .SetValidator(new AggregateCompleteTransactionValidator());
                    });
                When(transaction => transaction is AggregateBondedTransaction,
                    () =>
                    {
                        RuleFor(transaction => (AggregateBondedTransaction) transaction)
                            .SetValidator(new AggregateBondedTransactionValidator());
                    });
                When(transaction => transaction is TransferTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((TransferTransaction) transaction).InnerTransaction)
                            .SetValidator(new TransferTransactionValidator());
                    });
                When(transaction => transaction is MosaicDefinitionTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((MosaicDefinitionTransaction) transaction).InnerTransaction)
                            .SetValidator(new MosaicDefinitionTransactionValidator());
                    });
                When(transaction => transaction is MosaicSupplyChangeTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((MosaicSupplyChangeTransaction) transaction).InnerTransaction)
                            .SetValidator(new MosaicSupplyChangeTransactionValidator());
                    });
                When(transaction => transaction is MosaicSupplyRevocationTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((MosaicSupplyRevocationTransaction) transaction).InnerTransaction)
                            .SetValidator(new MosaicSupplyRevocationTransactionValidator());
                    });
                When(transaction => transaction is AccountKeyLinkTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((AccountKeyLinkTransaction) transaction).InnerTransaction)
                            .SetValidator(new AccountKeyLinkTransactionValidator());
                    });
                When(transaction => transaction is NodeKeyLinkTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((NodeKeyLinkTransaction) transaction).InnerTransaction)
                            .SetValidator(new NodeKeyLinkTransactionValidator());
                    });
                When(transaction => transaction is VotingKeyLinkTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((VotingKeyLinkTransaction) transaction).InnerTransaction)
                            .SetValidator(new VotingKeyLinkTransactionValidator());
                    });
                When(transaction => transaction is VrfKeyLinkTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((VrfKeyLinkTransaction) transaction).InnerTransaction)
                            .SetValidator(new VrfKeyLinkTransactionValidator());
                    });
                When(transaction => transaction is HashLockTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((HashLockTransaction) transaction).InnerTransaction)
                            .SetValidator(new HashLockTransactionValidator());
                    });
                When(transaction => transaction is SecretLockTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((SecretLockTransaction) transaction).InnerTransaction)
                            .SetValidator(new SecretLockTransactionValidator());
                    });
                When(transaction => transaction is SecretProofTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((SecretProofTransaction) transaction).InnerTransaction)
                            .SetValidator(new SecretProofTransactionValidator());
                    });
                When(transaction => transaction is AccountMetadataTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((AccountMetadataTransaction) transaction).InnerTransaction)
                            .SetValidator(new AccountMetadataTransactionValidator());
                    });
                When(transaction => transaction is MosaicMetadataTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((MosaicMetadataTransaction) transaction).InnerTransaction)
                            .SetValidator(new MosaicMetadataTransactionValidator());
                    });
                When(transaction => transaction is NamespaceMetadataTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((NamespaceMetadataTransaction) transaction).InnerTransaction)
                            .SetValidator(new NamespaceMetadataTransactionValidator());
                    });
                When(transaction => transaction is MultisigAccountModificationTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((MultisigAccountModificationTransaction) transaction).InnerTransaction)
                            .SetValidator(new MultisigAccountModificationTransactionValidator());
                    });
                When(transaction => transaction is AddressAliasTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((AddressAliasTransaction) transaction).InnerTransaction)
                            .SetValidator(new AddressAliasTransactionValidator());
                    });
                When(transaction => transaction is MosaicAliasTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((MosaicAliasTransaction) transaction).InnerTransaction)
                            .SetValidator(new MosaicAliasTransactionValidator());
                    });
                When(transaction => transaction is NamespaceRegistrationTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((NamespaceRegistrationTransaction) transaction).InnerTransaction)
                            .SetValidator(new NamespaceRegistrationTransactionValidator());
                    });
                When(transaction => transaction is AccountAddressRestrictionTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((AccountAddressRestrictionTransaction) transaction).InnerTransaction)
                            .SetValidator(new AccountAddressRestrictionTransactionValidator());
                    });
                When(transaction => transaction is AccountMosaicRestrictionTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((AccountMosaicRestrictionTransaction) transaction).InnerTransaction)
                            .SetValidator(new AccountMosaicRestrictionTransactionValidator());
                    });
                When(transaction => transaction is AccountOperationRestrictionTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((AccountOperationRestrictionTransaction) transaction).InnerTransaction)
                            .SetValidator(new AccountOperationRestrictionTransactionValidator());
                    });
                When(transaction => transaction is MosaicAddressRestrictionTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((MosaicAddressRestrictionTransaction) transaction).InnerTransaction)
                            .SetValidator(new MosaicAddressRestrictionTransactionValidator());
                    });
                When(transaction => transaction is MosaicGlobalRestrictionTransaction,
                    () =>
                    {
                        RuleFor(transaction => ((MosaicGlobalRestrictionTransaction) transaction).InnerTransaction)
                            .SetValidator(new MosaicGlobalRestrictionTransactionValidator());
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
    
    public class AggregateCompleteTransactionValidator : AbstractValidator<AggregateCompleteTransaction>
    {
        public AggregateCompleteTransactionValidator()
        {
            RuleForEach(transaction => transaction.Transactions.Values).SetValidator(new InnerTransactionValidator());
        }
    }
    
    private class AggregateBondedTransactionValidator : AbstractValidator<AggregateBondedTransaction>
    {
        public AggregateBondedTransactionValidator()
        {
            RuleForEach(transaction => transaction.Transactions).SetValidator(new InnerTransactionValidator());
        }
    }
    
    private class InnerTransactionValidator : AbstractValidator<IInnerTransaction>
    {
        public InnerTransactionValidator()
        {
            When(innerTransaction => innerTransaction is InnerTransferTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerTransferTransaction) innerTransaction).SetValidator(new TransferTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerMosaicDefinitionTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerMosaicDefinitionTransaction) innerTransaction).SetValidator(new MosaicDefinitionTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerMosaicSupplyChangeTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerMosaicSupplyChangeTransaction) innerTransaction).SetValidator(new MosaicSupplyChangeTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerMosaicSupplyRevocationTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerMosaicSupplyRevocationTransaction) innerTransaction).SetValidator(new MosaicSupplyRevocationTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerAccountKeyLinkTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerAccountKeyLinkTransaction) innerTransaction).SetValidator(new AccountKeyLinkTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerNodeKeyLinkTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerNodeKeyLinkTransaction) innerTransaction).SetValidator(new NodeKeyLinkTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerVotingKeyLinkTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerVotingKeyLinkTransaction) innerTransaction).SetValidator(new VotingKeyLinkTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerVrfKeyLinkTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerVrfKeyLinkTransaction) innerTransaction).SetValidator(new VrfKeyLinkTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerHashLockTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerHashLockTransaction) innerTransaction).SetValidator(new HashLockTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerSecretLockTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerSecretLockTransaction) innerTransaction).SetValidator(new SecretLockTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerSecretProofTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerSecretProofTransaction) innerTransaction).SetValidator(new SecretProofTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerAccountMetadataTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerAccountMetadataTransaction) innerTransaction).SetValidator(new AccountMetadataTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerMosaicMetadataTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerMosaicMetadataTransaction) innerTransaction).SetValidator(new MosaicMetadataTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerNamespaceMetadataTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerNamespaceMetadataTransaction) innerTransaction).SetValidator(new NamespaceMetadataTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerMultisigAccountModificationTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerMultisigAccountModificationTransaction) innerTransaction).SetValidator(new MultisigAccountModificationTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerAddressAliasTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerAddressAliasTransaction) innerTransaction).SetValidator(new AddressAliasTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerMosaicAliasTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerMosaicAliasTransaction) innerTransaction).SetValidator(new MosaicAliasTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerNamespaceRegistrationTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerNamespaceRegistrationTransaction) innerTransaction).SetValidator(new NamespaceRegistrationTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerAccountAddressRestrictionTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerAccountAddressRestrictionTransaction) innerTransaction).SetValidator(new AccountAddressRestrictionTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerAccountMosaicRestrictionTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerAccountMosaicRestrictionTransaction) innerTransaction).SetValidator(new AccountMosaicRestrictionTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerAccountOperationRestrictionTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerAccountOperationRestrictionTransaction) innerTransaction).SetValidator(new AccountOperationRestrictionTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerMosaicAddressRestrictionTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerMosaicAddressRestrictionTransaction) innerTransaction).SetValidator(new MosaicAddressRestrictionTransactionValidator());
            });
            When(innerTransaction => innerTransaction is InnerMosaicGlobalRestrictionTransaction, () =>
            {
                RuleFor(innerTransaction => (InnerMosaicGlobalRestrictionTransaction) innerTransaction).SetValidator(new MosaicGlobalRestrictionTransactionValidator());
            });
        }
    }
    
    private class MosaicValidator : AbstractValidator<Mosaic>
    {
        public MosaicValidator()
        {
            RuleFor(mosaic => mosaic.Id).NotEmpty().IsXCharacters(16).IsHex();
            RuleFor(mosaic => mosaic.Amount).NotEmpty().IsOver(0);
        }
    }
}