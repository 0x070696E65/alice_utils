using System.ComponentModel.DataAnnotations;

namespace aLice_utils.Shared.Models;

public class Mosaic
{
    public Mosaic(string id, string amount)
    {
        Id = id;
        Amount = amount;
    }
    
    public string Id { get; set; }
    public string Amount { get; set; }
}