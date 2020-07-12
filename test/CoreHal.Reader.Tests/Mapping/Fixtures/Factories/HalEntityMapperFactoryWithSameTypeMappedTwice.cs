using CoreHal.Reader.Mapping;
using CoreHal.Reader.Tests.Mapping.Fixtures.Mappers;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Factories
{
    public class HalEntityMapperFactoryWithSameTypeMappedTwice : EntityMapperFactory
    {
        public override void Configure(EntityMapperConfiguration cfg)
        {
            cfg
                .AddMapper(new MapperForExampleModel1())
                .AddMapper(new MapperForExampleModel1());
        }
    }
}