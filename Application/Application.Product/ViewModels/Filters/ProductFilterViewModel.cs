namespace Application.Product.ViewModels.Filters;

public class ProductFilterViewModel
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public decimal? PriceStart { get; set; }
    public decimal? PriceEnd { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTimeOffset? DateIncStart { get; set; }
    public DateTimeOffset? DateIncEnd { get; set; }
}