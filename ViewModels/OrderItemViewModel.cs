using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
  public class OrderItemViewModel
  {
    public int Id { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public int ProductId { get; set; }

    public string ProductCategory { get; set; } //using "Product" on the property name you are able to infer the name of sub-object on the mapping
    public string ProductSize { get; set; }
    public string ProductTitle { get; set; }
    public string ProductArtist { get; set; }
    public string ProductArtId { get; set; }
  }
}