using aLice_utils.Shared.Models.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace aLice_utils.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController : Controller
{
    [HttpGet("{id}")]
    public ActionResult<string>? GetById(string id)
    {
        if (id == "TransferTransaction")
        {
            return "TransferTransaction";
        }
        return null;
    }
}