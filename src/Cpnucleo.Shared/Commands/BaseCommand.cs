using MessagePack;

namespace Cpnucleo.Shared.Commands;

[MessagePackObject(true)]
public abstract record BaseCommand();