namespace Application.UseCases.User.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<GetUserByIdQueryViewModel>;
