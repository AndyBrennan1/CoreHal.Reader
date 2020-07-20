using CoreHal.Reader.Mapping;
using CoreHal.Reader.Tests.Fixtures;
using System;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Mappers
{
    public class MapperForComplexModel : EntityMapper<ComplexModel>
    {
        public override ComplexModel Map()
        {
            Entity.Id = MapTo<Guid>("id");
            Entity.Name = MapTo<string>("full-Name");
            Entity.Date = MapTo<DateTime>("date");

            return Entity;
        }
    }
}