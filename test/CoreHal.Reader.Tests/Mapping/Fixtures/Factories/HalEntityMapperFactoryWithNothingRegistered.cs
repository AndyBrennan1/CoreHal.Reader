using CoreHal.Reader.Mapping;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Factories
{
    public class HalEntityMapperFactoryWithNothingRegistered : EntityMapperFactory
    {
        public override void Configure(EntityMapperConfiguration cfg)
        {
        }
    }
}