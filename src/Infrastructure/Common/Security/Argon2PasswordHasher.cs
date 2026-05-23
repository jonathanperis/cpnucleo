namespace Infrastructure.Common.Security;

using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

public sealed class Argon2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int MemorySize = 65_536;
    private const int Iterations = 3;
    private const int DegreeOfParallelism = 2;
    private const int MaxMemorySize = 262_144;
    private const int MaxIterations = 10;
    private const int MaxDegreeOfParallelism = 8;
    private const string Argon2idPrefix = "$argon2id$";

    public PasswordHash Hash(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return new PasswordHash(string.Empty, string.Empty);
        }

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = HashArgon2id(password, salt, MemorySize, Iterations, DegreeOfParallelism, HashSize);
        var phc = $"$argon2id$v=19$m={MemorySize},t={Iterations},p={DegreeOfParallelism}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";

        return new PasswordHash(phc, string.Empty);
    }

    public bool Verify(string? password, string? hash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
        {
            return false;
        }

        if (!hash.StartsWith(Argon2idPrefix, StringComparison.Ordinal))
        {
            return false;
        }

        try
        {
            return VerifyArgon2id(password, hash);
        }
        catch (FormatException)
        {
            return false;
        }
        catch (OverflowException)
        {
            return false;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private static byte[] HashArgon2id(string password, byte[] salt, int memorySize, int iterations, int degreeOfParallelism, int hashSize)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            MemorySize = memorySize,
            Iterations = iterations,
            DegreeOfParallelism = degreeOfParallelism
        };

        return argon2.GetBytes(hashSize);
    }

    private static bool VerifyArgon2id(string password, string phcHash)
    {
        var parts = phcHash.Split('$', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 5 || parts[0] != "argon2id" || parts[1] != "v=19")
        {
            return false;
        }

        var parameters = ParseParameters(parts[2]);
        if (!ParametersAreAllowed(parameters))
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[3]);
        var expectedHash = Convert.FromBase64String(parts[4]);
        var actualHash = HashArgon2id(
            password,
            salt,
            parameters.MemorySize,
            parameters.Iterations,
            parameters.DegreeOfParallelism,
            expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }

    private static Argon2Parameters ParseParameters(string value)
    {
        var parameters = value
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split('=', 2))
            .ToDictionary(x => x[0], x => int.Parse(x[1]));

        return new Argon2Parameters(
            parameters["m"],
            parameters["t"],
            parameters["p"]);
    }

    private static bool ParametersAreAllowed(Argon2Parameters parameters)
    {
        return parameters.MemorySize is > 0 and <= MaxMemorySize
            && parameters.Iterations is > 0 and <= MaxIterations
            && parameters.DegreeOfParallelism is > 0 and <= MaxDegreeOfParallelism;
    }

    private sealed record Argon2Parameters(int MemorySize, int Iterations, int DegreeOfParallelism);
}
