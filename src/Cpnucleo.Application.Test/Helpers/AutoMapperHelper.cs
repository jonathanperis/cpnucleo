﻿namespace Cpnucleo.Application.Test.Helpers;

public class AutoMapperHelper
{
    public static IMapper GetMappings()
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.AddProfile<EntityToDTOProfile>();
        });

        return config.CreateMapper();
    }
}
