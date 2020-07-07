using CoreHal.Reader.Mapping;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;
using System;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Mappers
{
    public class MapperForExampleModel2 : EntityMapper<ExampleModelWithMapping2>
    {
        public override ExampleModelWithMapping2 Map()
        {
            Entity.DateProperty = MapTo<DateTime>("date-Property");
            Entity.GuidProperty = MapTo<Guid>("guid-Property");

            return Entity;
        }
    }
}