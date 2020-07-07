using CoreHal.Reader.Mapping;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Mappers
{
    public class MapperMapsComplexPropertyAsSimpleOne : EntityMapper<ExampleModelWithMapping1>
    {
        public override ExampleModelWithMapping1 Map()
        {
            Entity.IntegerProperty = MapTo<int>("complex-Property");

            return Entity;
        }
    }
}