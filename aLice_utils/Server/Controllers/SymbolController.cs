using CatSdk.CryptoTypes;
using CatSdk.Facade;
using CatSdk.Symbol;
using Microsoft.AspNetCore.Mvc;

namespace aLice_utils.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SymbolController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        var keyPair = KeyPair.GenerateNewKeyPair();
        var facade = new SymbolFacade(Network.TestNet);
        var address = facade.Network.PublicKeyToAddress(keyPair.PublicKey).ToString();
        return address;
    }
}