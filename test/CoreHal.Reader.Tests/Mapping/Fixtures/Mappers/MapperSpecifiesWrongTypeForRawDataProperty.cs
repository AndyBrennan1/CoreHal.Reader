using CoreHal.Reader.Mapping;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Mappers
{
    public class MapperSpecifiesWrongTypeForRawDataProperty : EntityMapper<ExampleModelWithMapping1>
    {
        public override ExampleModelWithMapping1 Map()
        {
            Entity.StringProperty = MapTo<string>("integer-Property");

            return Entity;
        }
    }
}