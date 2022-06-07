using Cpnucleo.Application.Common.Mappings;

namespace Cpnucleo.Application.Test.Helpers;

public class AutoMapperHelper
{
    public static IMapper GetMappings()
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.AddProfile<DTOToEntityProfile>();
            cfg.AddProfile<EntityToDTOProfile>();
            cfg.AddProfile<CommandToEntityProfile>();
        });

        return config.CreateMapper();
    }
}
