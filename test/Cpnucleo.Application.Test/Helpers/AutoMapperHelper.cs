using Cpnucleo.Application.Configuration;

namespace Cpnucleo.Application.Test.Helpers;

public class AutoMapperHelper
{
    public static IMapper GetMappings()
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ViewModelToEntityProfile>();
            cfg.AddProfile<EntityToViewModelProfile>();
        });

        return config.CreateMapper();
    }
}
