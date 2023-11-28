using aLice_utils.Client.Extensions;
using aLice_utils.Shared.Models;
using FluentValidation;
    
namespace aLice_utils.Client.Services;

public abstract class Validator
{
    public class TransferTransactionValidator : AbstractValidator<aLice_utils.Shared.Models.Transaction.TransferTransaction>
    {
        public TransferTransactionValidator()
        {
            RuleFor(tranasction => tranasction.TransactionMeta.Deadline).NotEmpty().IsOver(10).IsUnder(7200);
            RuleFor(tranasction => tranasction.TransactionMeta.Node).NotEmpty().IsNodeUrl();

            RuleFor(tranasction => tranasction.InnerTransaction.Message).IsUnderXByte(1023);
            RuleFor(tranasction => tranasction.InnerTransaction.RecipientAddress).NotEmpty().IsSymbolAddress();
            RuleForEach(transaction => transaction.InnerTransaction.Mosaics).SetValidator(new MosaicValidator());
        }
    }
    
    public class MosaicDefinitionTransactionValidator : AbstractValidator<aLice_utils.Shared.Models.Transaction.MosaicDefinitionTransaction>
    {
        public MosaicDefinitionTransactionValidator()
        {
            RuleFor(tranasction => tranasction.TransactionMeta.Deadline).NotEmpty().IsOver(10).IsUnder(7200);
            RuleFor(tranasction => tranasction.TransactionMeta.Node).NotEmpty().IsNodeUrl();
            RuleFor(tranasction => tranasction.InnerTransaction.SignerPublicKey).NotEmpty().IsHex().IsXCharacters(64);
            RuleFor(tranasction => tranasction.InnerTransaction.MosaicID);
            RuleFor(tranasction => tranasction.InnerTransaction.Duration).NotEmpty().IsOver(0);
            RuleFor(tranasction => tranasction.InnerTransaction.Divisibility).NotEmpty().IsOver(0).IsUnder(6);
        }
    }
    
    private class MosaicValidator : AbstractValidator<Mosaic>
    {
        public MosaicValidator()
        {
            RuleFor(mosaic => mosaic.Id).NotEmpty();// .IsXCharacters(16).IsHex();
            RuleFor(mosaic => mosaic.Amount).NotEmpty().IsOver(0);
        }
    }
}