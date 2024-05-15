using Cqrs.Core.Queries;

namespace NedoSilpo.Query.Api.Queries;

public record FilterProductsQuery(string Name, decimal PriceMin, decimal PriceMax, string Description = "") : BaseQuery;

public record GetByIdQuery(Guid Id) : BaseQuery;
