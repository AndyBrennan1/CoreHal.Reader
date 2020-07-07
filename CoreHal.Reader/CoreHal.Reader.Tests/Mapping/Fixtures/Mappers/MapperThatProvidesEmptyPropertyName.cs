using CoreHal.Reader.Mapping;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Mappers
{
    public class MapperThatProvidesEmptyPropertyName : EntityMapper<ExampleModelWithMapping1>
    {
        public override ExampleModelWithMapping1 Map()
        {
            Entity.IntegerProperty = MapTo<int>(string.Empty);

            return Entity;
        }
    }
}