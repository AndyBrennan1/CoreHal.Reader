using System;
using System.Threading.Tasks;

namespace CoreHal.Reader
{
    public interface IRESTApi
    {
        Task<IHalResource> GetResource(Uri resourceUri);
        Task<IHalResource> Create<TEntity>(Uri url, TEntity resource);
    }
}