using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Interfaces
{
    public interface ICrudApiService<TViewModel>
    {
        IEnumerable<TViewModel> Get(string token, string actionRoute);

        TViewModel Get(string token, string actionRoute, Guid id);

        void Post(string token, string actionRoute, TViewModel obj);

        void Put(string token, string actionRoute, Guid id, TViewModel obj);

        void Delete(string token, string actionRoute, Guid id);
    }
}
