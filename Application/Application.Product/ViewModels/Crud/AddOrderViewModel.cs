namespace Application.Product.ViewModels.Crud;

public class AddOrderViewModel
{
    public IEnumerable<AddOrderItemViewModel> AddOrderItems { get; set; }
}

public class AddOrderItemViewModel
{
    public Guid ProductId { get; set; }
    public decimal ListPrice { get; set; }
    public decimal Discount { get; set; }
    public int Quantity { get; set; }
}