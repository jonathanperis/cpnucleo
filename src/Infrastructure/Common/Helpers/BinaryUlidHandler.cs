namespace Infrastructure.Common.Helpers;

//For Dapper Ulid Integration
public sealed class BinaryUlidHandler : TypeHandler<Ulid>
{
    public override Ulid Parse(DbParameter parameter)
    {
        return new Ulid((byte[])parameter.Value!);
        // return base.Parse(parameter);
    }

    // public override Ulid Parse(object value)
    // {
    //     return new Ulid((byte[])value);
    // }

    public override void SetValue(DbParameter parameter, Ulid value)
    {
        parameter.DbType = DbType.Binary;
        parameter.Size = 16;
        parameter.Value = value.ToByteArray();

        // base.SetValue(parameter, value);
    }

    // public override void SetValue(IDbDataParameter parameter, Ulid value)
    // {
    //     parameter.DbType = DbType.Binary;
    //     parameter.Size = 16;
    //     parameter.Value = value.ToByteArray();
    // }
}
