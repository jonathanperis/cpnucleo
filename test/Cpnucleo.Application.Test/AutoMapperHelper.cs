using AutoMapper;
using Cpnucleo.Application.Configuration;

namespace Cpnucleo.Application.Test
{
    public class AutoMapperHelper
    {
        public static IMapper GetMappings()
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<ViewModelToEntityProfile>();
                cfg.AddProfile<EntityToViewModelProfile>();
            });

            return config.CreateMapper();
        }
    }
}
