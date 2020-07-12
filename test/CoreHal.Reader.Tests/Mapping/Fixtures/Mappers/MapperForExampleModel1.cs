using CoreHal.Reader.Mapping;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Mappers
{
    public class MapperForExampleModel1 : EntityMapper<ExampleModelWithMapping1>
    {
        public override ExampleModelWithMapping1 Map()
        {
            Entity.IntegerProperty = MapTo<int>("integer-Property");
            Entity.StringProperty = MapTo<string>("string-Property");
            
            return Entity;
        }
    }
}