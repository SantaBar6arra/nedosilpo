namespace Cqrs.Core.Exceptions;

public class AggregateNotFoundException(string? message) : Exception(message);
