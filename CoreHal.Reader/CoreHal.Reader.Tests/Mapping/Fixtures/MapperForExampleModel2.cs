using CoreHal.Reader.Mapping;
using System;
using System.Collections.Generic;

namespace CoreHal.Reader.Tests.Mapping.Fixtures
{
    public class MapperForExampleModel2 : HalEntityMapper<ExampleModelWithMapping2>
    {
        public override ExampleModelWithMapping2 Map(IDictionary<string, object> raw)
        {
            entity.DateProperty = MapTo<DateTime>("date-Property");
            entity.GuidProperty = MapTo<Guid>("guid-Property");

            return entity;
        }
    }
}