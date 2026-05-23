namespace Domain.Common.Security;

public sealed record PasswordHash(string Hash, string Salt);
