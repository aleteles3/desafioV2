using Domain.Core.Entities;
using Domain.Product.Entities;

namespace Domain.Product.Shared;

public sealed class User : Entity<User>
{
    //Properties
    public string Login { get; }
    public string Password { get; }
    public bool Status { get; }
    
    //Navigation Properties
    public IEnumerable<Order> Orders { get; set; }
    
    public override bool IsValid()
    {
        throw new NotImplementedException();
    }
}