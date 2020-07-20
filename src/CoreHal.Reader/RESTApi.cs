using CoreHal.Reader.Loading;
using CoreHal.Reader.Mapping;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreHal.Reader
{
    /// <summary>
    /// 
    /// </summary>
    public class RESTApi : IRESTApi
    {
        private readonly IHalResponseLoader responseLoader;
        private readonly IEntityMapperFactory entityMapperFactory;

        public RESTApi(IHalResponseLoader responseLoader, IEntityMapperFactory entityMapperFactory)
        {
            this.responseLoader = responseLoader;
            this.entityMapperFactory = entityMapperFactory;
            this.entityMapperFactory.RegisterMappers();
        }

        public async Task<IHalResource> GetResource(Uri resourceUri)
        {
            string jsonString;

            using (var client = new HttpClient())
            {
                client.BaseAddress = resourceUri;

                var result = await client.GetAsync(resourceUri.ToString());

                jsonString = await result.Content.ReadAsStringAsync();
            }

            var halResource = new HalResource(responseLoader, entityMapperFactory);
            halResource.Load(jsonString);

            return halResource;
        }

    }
}