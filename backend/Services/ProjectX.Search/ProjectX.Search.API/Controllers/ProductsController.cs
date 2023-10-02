using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using ProjectX.Search.API.Models;

namespace ProjectX.Search.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IElasticClient _client;

    public ProductsController(IElasticClient client)
    {
        _client = client;
    }


    [HttpGet]
    public async Task<IActionResult> GetProducts(string keyword)
    {
        var results = await _client.SearchAsync<Product>(
            s => s.Index("product").Query(q => 
                q.QueryString(d => d.Query('*'+keyword+'*'))           
            )
            .Size(1000) // Take
        );

        return Ok(results.Documents.ToList());
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product) 
    {
        await _client.IndexAsync(product, x => x.Index("product"));

        return Ok();
    }
}
