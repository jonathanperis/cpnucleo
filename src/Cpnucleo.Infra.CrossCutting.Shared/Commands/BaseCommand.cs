﻿using MessagePack;

namespace Cpnucleo.Infra.CrossCutting.Shared.Commands;

[MessagePackObject(true)]
public abstract record BaseCommand();