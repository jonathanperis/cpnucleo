namespace Application.UseCases.User.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : BaseQuery, IRequest<GetUserByIdQueryViewModel>;
