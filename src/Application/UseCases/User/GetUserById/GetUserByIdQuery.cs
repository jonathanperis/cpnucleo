namespace Application.UseCases.User.GetUserById;

public sealed record GetUserByIdQuery(Ulid Id) : IRequest<GetUserByIdQueryViewModel>;
