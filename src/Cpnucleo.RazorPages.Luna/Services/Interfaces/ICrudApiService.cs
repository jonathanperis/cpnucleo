using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Luna.Services.Interfaces
{
    public interface ICrudApiService<TViewModel>
    {
        IEnumerable<TViewModel> Get(string actionRoute);

        TViewModel Get(string actionRoute, string parameter);

        void Post(string actionRoute, TViewModel obj);

        void Put(string actionRoute, string parameter, TViewModel obj);

        void Delete(string actionRoute, string parameter);
    }
}
