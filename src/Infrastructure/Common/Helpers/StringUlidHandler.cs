namespace Infrastructure.Common.Helpers;

//For Dapper Ulid Integration
public sealed class StringUlidHandler : TypeHandler<Ulid>
{
    public override Ulid Parse(DbParameter parameter)
    {
        return Ulid.Parse((string)parameter.Value!);
        // return base.Parse(parameter);
    }

    // public override Ulid Parse(object value)
    // {
    //     return Ulid.Parse((string)value);
    // }

    public override void SetValue(DbParameter parameter, Ulid value)
    {
        parameter.DbType = DbType.StringFixedLength;
        parameter.Size = 26;
        parameter.Value = value.ToString();

        // base.SetValue(parameter, value);
    }

    // public override void SetValue(IDbDataParameter parameter, Ulid value)
    // {
    //     parameter.DbType = DbType.StringFixedLength;
    //     parameter.Size = 26;
    //     parameter.Value = value.ToString();
    // }
}
