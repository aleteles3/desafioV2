using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Service.Product.Controllers;

[Authorize]
[Route("api/Product/Order")]
public class OrderController
{
    
}