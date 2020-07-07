using CoreHal.Reader.Mapping;
using System.Collections.Generic;

namespace CoreHal.Reader.Tests.Mapping.Fixtures
{
    public class MapperForExampleModel1 : HalEntityMapper<ExampleModelWithMapping1>
    {
        public override ExampleModelWithMapping1 Map(IDictionary<string, object> raw)
        {
            entity.IntegerProperty = MapTo<int>("integer-Property");
            entity.StringProperty = MapTo<string>("string-Property");

            return entity;
        }
    }
}