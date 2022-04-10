namespace Application.Product.ViewModels.Filters;

public class CategoryFilterViewModel
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset? DateIncStart { get; set; }
    public DateTimeOffset? DateIncEnd { get; set; }
}