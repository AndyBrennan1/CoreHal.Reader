using CoreHal.Reader.Mapping;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;

namespace CoreHal.Reader.Tests.Mapping.Fixtures.Mappers
{
    public class MapperThatConfiguresComplexFieldWithNullPropertyName : EntityMapper<ExampleModelWithComplexProperty>
    {
        public override ExampleModelWithComplexProperty Map()
        {
            Entity.ComplexProperty = 
                this.MapToComplex<ExampleModelWithMapping1>(null)
                .Map(rawData => new ExampleModelWithMapping1
                {
                    IntegerProperty = (int)rawData["integer-Property"],
                    StringProperty = (string)rawData["string-Property"]
                });

            return Entity;
        }
    }
}