using Cqrs.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NedoSilpo.Command.Api.Commands;
using NedoSilpo.Command.Domain.Dtos;

namespace NedoSilpo.Command.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController
{
    private readonly ILogger<ProductController> _logger;
    private readonly ICommandDispatcher _dispatcher;

    public ClientController(ILogger<ProductController> logger, ICommandDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterClientRequest request)
    {
        try
        {
            var (fullName, email, address, phoneNumber) = request;
            var command = new RegisterClient(fullName, email, address, phoneNumber);
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
    public async Task<ActionResult> Update(UpdateClientRequest request)
    {
        try
        {
            var (id, fullName, email, address, phoneNumber) = request;
            var command = new UpdateClient(id, fullName, email, address, phoneNumber);
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

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<ActionResult> Deactivate(Guid id)
    {
        try
        {
            var command = new DeactivateClient(id);
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
