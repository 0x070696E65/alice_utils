namespace aLice_utils.Shared.Models.Transaction;

public class InnerAccountMetadataTransaction: IInnerTransaction
{
    public string SignerPublicKey { get; set; } = "";
    public string TargetAddress { get; set; } = "";
    public string ScopedMetadataKey { get; set; } = "";
    public string Value { get; set; } = "";
    public ulong UlongScopedMetadataKey { get; set; }
    public byte[] ValueBytes { get; set; } = Array.Empty<byte>();
    public string Node { get; set; } = "";
    public ushort XorValueSize { get; set; }
}