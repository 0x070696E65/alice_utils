using aLice_utils.Shared.Models.Transaction;
using CatSdk.Symbol;
using CatSdk.Utils;
using NETWORKTYPE = CatSdk.Symbol.NetworkType;

namespace aLice_utils.Client.Services;

public abstract class SymbolBuilderService
{
    public static ITransaction BuildTransaction(BaseTransaction transaction)
    {
        if (transaction.GetType() == typeof(TransferTransaction))
        {
            if (transaction is TransferTransaction t)
            {
                return BuildTransferTransaction(t);
            }
        } else if (transaction.GetType() == typeof(MosaicDefinitionTransaction))
        {
            if (transaction is MosaicDefinitionTransaction t)
            {
                return BuildMosaicDefinitionTransaction(t);
            }
        }
        throw new Exception("transaction is not defined.");
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
}