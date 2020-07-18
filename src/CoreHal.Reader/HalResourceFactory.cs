using CoreHal.Reader.Loading;
using CoreHal.Reader.Mapping;

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