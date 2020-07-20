using CoreHal.Reader.Mapping;
using CoreHal.Reader.Tests.Mapping.Fixtures.Mappers;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Factories
{
    public class HalEntityMapperFactoryWith2TypesRegistered : EntityMapperFactory
    {
        public override void Configure(EntityMapperConfiguration cfg)
        {
            cfg
                .AddMapper<ExampleModelWithMapping1, MapperForExampleModel1>()
                .AddMapper<ExampleModelWithMapping2, MapperForExampleModel2>();
        }
    }
}