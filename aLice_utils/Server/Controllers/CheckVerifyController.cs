using CatSdk.Symbol;
using CatSdk.Utils;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace aLice_utils.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckVerifyController : ControllerBase
{
    [HttpPost]
    public bool Post([FromBody] Dictionary<string, string> data)
    {
        if (data == null) throw new Exception("data is not correct format");
        if (!data.ContainsKey("message")) throw new Exception("message is nothing");
        if (!data.ContainsKey("hash")) throw new Exception("hash is nothing");
        if (!data.ContainsKey("public_key")) throw new Exception("public_key is nothing");
        var message = data["message"];
        var hash = data["hash"];
        var public_key = data["public_key"];
        
        var signature = new Signature(Converter.HexToBytes(hash));
        var ed25519Signer = new Ed25519Signer();
        ed25519Signer.Init(false, (ICipherParameters) new Ed25519PublicKeyParameters(Converter.HexToBytes(public_key), 0));
        ed25519Signer.BlockUpdate(Converter.Utf8ToBytes(message), 0, Converter.Utf8ToBytes(message).Length);
        return ed25519Signer.VerifySignature(signature.bytes);
    }
}