using CoreHal.Reader.Mapping;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Mappers
{
    public class MapperThatProvidesNullPropertyName : EntityMapper<ExampleModelWithMapping1>
    {
        public override ExampleModelWithMapping1 Map()
        {
            Entity.IntegerProperty = MapTo<int>(null);

            return Entity;
        }
    }
}