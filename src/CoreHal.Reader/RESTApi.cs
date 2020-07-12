using CoreHal.Reader.Mapping;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace CoreHal.Reader
{
    public class RESTApi
    {
        private readonly IHalResource halResource;

        public Uri ApiRoute { get; set; }

        private readonly IHalResponseLoader responseLoader;
        private readonly IEntityMapperFactory entityMapperFactory;

        public RESTApi(IHalResponseLoader responseLoader, IEntityMapperFactory entityMapperFactory)
        {
            this.responseLoader = responseLoader;
            this.entityMapperFactory = entityMapperFactory;
        }

        public HalResource GetResource(Uri resourceUri)
        {
            HttpResponseMessage result;
            string jsonString;
            IDictionary<string, object> dict;

            using (var c = new HttpClient())
            {
                c.BaseAddress = resourceUri;
                result = c.GetAsync(resourceUri.ToString()).GetAwaiter().GetResult();
                jsonString = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                dict =JsonSerializer.Deserialize<IDictionary<string, object>>(jsonString);
            }

            var halResource = new HalResource(responseLoader, entityMapperFactory);

            return halResource;
        }

    }
}