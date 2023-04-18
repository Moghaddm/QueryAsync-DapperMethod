using Microsoft.AspNetCore.Mvc;

namespace query.Controllers;

[ApiController]
[Route("[controller]")]
public class DapperController : ControllerBase
{
    [HttpGet("[action]/{id}", Name = nameof(Get))]
    public async Task<IEnumerable<Product>> Get(int id)
    {
        var query = "SELECT * FROM PRODUCTS WHERE ID = @id ";
        return await DapperExtensions<Product>.QueryAsync(query, new { id = id });
    }

    // public async Task<IEnumerable<Product>> GetAll()
    // {
    //     var query = "SELECT & FROM PRODUCTS";
    //     return await DapperExtensions<Product>.QueryAsync(query);
    // }
}
