namespace Application.UseCases.System.GetSystemById;

public sealed record GetSystemByIdQuery(Ulid Id) : BaseQuery, IRequest<GetSystemByIdQueryViewModel>;
