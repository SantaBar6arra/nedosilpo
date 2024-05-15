using Microsoft.AspNetCore.Mvc;
using NedoSilpo.Query.Api.Queries;
using NedoSilpo.Query.Domain.Repositories;

namespace NedoSilpo.Query.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository) => _productRepository = productRepository;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FilterProductsQuery query)
    {
        var products = await _productRepository
            .GetAllAsync(query.Name, query.Description, query.PriceMin, query.PriceMax);
        return new JsonResult(products);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] GetByIdQuery query)
    {
        var product = await _productRepository.GetByIdAsync(query.Id);
        return new JsonResult(product);
    }

    [HttpGet("deleted")]
    public async Task<IActionResult> GetDeleted()
    {
        var products = await _productRepository.GetDeletedAsync();
        return new JsonResult(products);
    }
}
