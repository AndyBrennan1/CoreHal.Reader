using CoreHal.Reader.Mapping;

namespace CoreHal.Reader.Tests.Mapping.Fixtures
{
    public class HalEntityMapperFactoryWith2TypesRegistered : HalEntityMapperFactory
    {
        public override void Configure(HalEntityMapperConfiguration cfg)
        {
            cfg
                .AddMapper(new MapperForExampleModel1())
                .AddMapper(new MapperForExampleModel2());
        }
    }
}