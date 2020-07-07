using CoreHal.Reader.Mapping;

namespace CoreHal.Reader.Tests.Mapping.Fixtures
{
    public class HalEntityMapperFactoryWithNothingRegistered : HalEntityMapperFactory
    {
        public override void Configure(HalEntityMapperConfiguration cfg)
        {
        }
    }
}