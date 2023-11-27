using aLice_utils.Client.Extensions;
using aLice_utils.Shared.Models;
using global::FluentValidation;
using BlazorStrap.Extensions.FluentValidation;
    
namespace aLice_utils.Client.Services;

public abstract class Validator
{
    public class TransferTransactionValidator : AbstractValidator<aLice_utils.Shared.Models.Transaction.TransferTransaction>
    {
        public TransferTransactionValidator()
        {
            RuleFor(tranasction => tranasction.TransactionMeta.Deadline).NotEmpty().IsDeadline();
            RuleFor(tranasction => tranasction.TransactionMeta.Node).NotEmpty().IsNodeUrl();

            RuleFor(tranasction => tranasction.InnerTransferTransaction.Message).IsUnder1023Byte();
            RuleFor(tranasction => tranasction.InnerTransferTransaction.RecipientAddress).NotEmpty().IsSymbolAddress();
            RuleForEach(transaction => transaction.InnerTransferTransaction.Mosaics).SetValidator(new MosaicValidator());
        }
    }
    
    private class MosaicValidator : AbstractValidator<Mosaic>
    {
        public MosaicValidator()
        {
            RuleFor(mosaic => mosaic.Id).NotEmpty().IsMosaicId16().IsMosaicIdHex();
            RuleFor(mosaic => mosaic.Amount).NotEmpty();
        }
    }
}