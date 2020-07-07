using System;
using System.Collections.Generic;

namespace CoreHal.Reader.Mapping
{ 
    public class ComplexModel1Mapper : HalEntityMapper<ComplexModel1>
    {
        private readonly IHalEntityMapper<ComplexModel2> complex2Mapper;

        public ComplexModel1Mapper(IHalEntityMapper<ComplexModel2> complex2Mapper) : base()
        {
            this.complex2Mapper = complex2Mapper;
        }

        public override ComplexModel1 Map(IDictionary<string, object> raw)
        {
            rawData = raw;

            entity.Id = MapTo<Guid>("id");
            entity.Name = MapTo<string>("full-name");
            entity.Date = MapTo<DateTime>("date");
            entity.Complex = complex2Mapper.Map((IDictionary<string, object>)raw["Complex"]);

            return entity;
        }
    }

    public class ComplexModel2Mapper : HalEntityMapper<ComplexModel2>
    {
        public override ComplexModel2 Map(IDictionary<string, object> raw)
        {
            rawData = raw;

            entity.Id = MapTo<Guid>("id");
            entity.Name = MapTo<string>("full-name");
            entity.Description = MapTo<string>("description");

            return entity;
        }
    }

    public class ComplexModel1
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public ComplexModel2 Complex { get; set; }
    }

    public class ComplexModel2
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
