using System;
using System.Collections.Generic;

namespace CoreHal.Reader.Mapping
{ 
    public class ComplexModel1Mapper : EntityMapper<ComplexModel1>
    {
        private readonly IEntityMapper<ComplexModel2> complex2Mapper;

        public ComplexModel1Mapper(IEntityMapper<ComplexModel2> complex2Mapper) : base()
        {
            this.complex2Mapper = complex2Mapper;
        }

        public override ComplexModel1 Map()
        {
            Entity.Id = MapTo<Guid>("id");
            Entity.Name = MapTo<string>("full-name");
            Entity.Date = MapTo<DateTime>("date");

            complex2Mapper.LoadData(GetPropertyAsDictionary("Complex"));
            Entity.Complex = complex2Mapper.Map();



            return Entity;
        }
    }

    public class ComplexModel2Mapper : EntityMapper<ComplexModel2>
    {
        public override ComplexModel2 Map()
        {
            Entity.Id = MapTo<Guid>("id");
            Entity.Name = MapTo<string>("full-name");
            Entity.Description = MapTo<string>("description");

            return Entity;
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
