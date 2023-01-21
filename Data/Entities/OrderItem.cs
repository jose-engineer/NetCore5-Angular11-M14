namespace DutchTreat.Data.Entities
{
  public class OrderItem
  {
    public int Id { get; set; }
    public Product Product { get; set; }
    public int ProductId { get; set; } //we create this cus we don't want to create the new object, allowing that mapping actually happened. It really has to do with going from order model to order in our mapping, we dont' want to bring those products in as if they are new products, we just want to keep the product
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Order Order { get; set; }
  }
}