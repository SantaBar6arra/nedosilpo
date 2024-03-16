using Cqrs.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NedoSilpo.Command.Api.Commands;
using NedoSilpo.Command.Domain.Dtos;

namespace NedoSilpo.Command.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController
{
    private readonly ILogger<ProductController> _logger;
    private readonly ICommandDispatcher _dispatcher;

    public ProductController(ILogger<ProductController> logger, ICommandDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateProductRequest request)
    {
        try
        {
            var (name, description, price, quantityAvailable) = request;
            var command = new CreateProduct(name, description, price, quantityAvailable);
            await _dispatcher.SendAsync(command);
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
        catch (InvalidOperationException exception)
        {
            _logger.LogError(exception.Message);
            return new ObjectResult(new { exception.Message }) { StatusCode = StatusCodes.Status400BadRequest };
        }
        catch (Exception)
        {
            return new ObjectResult("Internal server error") { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }

    [HttpPut]
    public async Task<ActionResult> Update(UpdateProductRequest request)
    {
        try
        {
            var (id, name, description, price, quantityAvailable) = request;
            var command = new UpdateProduct(id, name, description, price, quantityAvailable);
            await _dispatcher.SendAsync(command);
            return new StatusCodeResult(StatusCodes.Status200OK);
        }
        catch (InvalidOperationException exception)
        {
            _logger.LogError(exception.Message);
            return new ObjectResult(new { exception.Message }) { StatusCode = StatusCodes.Status400BadRequest };
        }
        catch (Exception)
        {
            return new ObjectResult("Internal server error") { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
