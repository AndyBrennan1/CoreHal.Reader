using CoreHal.Reader.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreHal.Reader
{
    public class HalResourceFactory
    {
        private readonly IHalResponseLoader responseLoader;
        private readonly IEntityMapperFactory entityMapperFactory;

        public HalResourceFactory(IHalResponseLoader responseLoader, IEntityMapperFactory entityMapperFactory)
        {
            this.responseLoader = responseLoader;
            this.entityMapperFactory = entityMapperFactory;
        }

        public HalResource GetHalResource()
        {
            var halResource = new HalResource(responseLoader, entityMapperFactory);

            return halResource;
        }
    }
}
