using MessagePack;

namespace Cpnucleo.Infra.CrossCutting.Shared.Queries;

[MessagePackObject(true)]
public abstract record BaseQuery();