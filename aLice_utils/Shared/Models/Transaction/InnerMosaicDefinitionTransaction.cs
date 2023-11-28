using System.ComponentModel;

namespace aLice_utils.Shared.Models.Transaction;

public class InnerMosaicDefinitionTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string MosaicID { get; set; } = "";
    
    public uint Nonce { get; set; } = 0;
    public string Duration { get; set; } = "";
    public string Divisibility { get; set; } = "";
    public string SupplyMutable { get; set; } = "false";
    public string Transferable { get; set; } = "false";
    public string Restrictable { get; set; } = "false";
    public string Revokable { get; set; } = "false";
}