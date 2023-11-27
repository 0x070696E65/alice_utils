using System.IO;
using QRCoder;
    
namespace aLice_utils.Client.Services;

public class QRServices
{
    public static async Task<string> GenerateQR(string url)
    {
        return await Task.Run(() => // バックグラウンドで処理を行う
        {
            using var ms = new MemoryStream();
            var qrCodeGenerate = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerate.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var pngByteQRCode = new PngByteQRCode(qrCodeData);
            var base64 = Convert.ToBase64String(pngByteQRCode.GetGraphic(10));
            return $"data:image/png;base64,{base64}";
        });
    }
}