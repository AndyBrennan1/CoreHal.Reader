using CoreHal.Reader.Mapping;
using CoreHal.Reader.Mapping.Exceptions;
using CoreHal.Reader.Tests.Mapping.Fixtures.Mappers;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;
using System;
using Xunit;

namespace CoreHal.Reader.Tests.Mapping
{
    public class EntityMapperConfigurationTests
    {
        [Fact]
        public void Constructing_InitializesMappersDictionary()
        {
            var configuration = new EntityMapperConfiguration();

            Assert.NotNull(configuration.Mappers);
        }

        [Fact]
        public void AddingMapper_WithMapperProvided_AddsItToTheMapperDictionary()
        {
            var configuration = new EntityMapperConfiguration();

            configuration.AddMapper<ExampleModelWithMapping1, MapperForExampleModel1>();

            Assert.Single(configuration.Mappers);

            Assert.Equal(
                expected: typeof(MapperForExampleModel1), 
                actual: configuration.Mappers[typeof(ExampleModelWithMapping1)]);
        }

        [Fact]
        public void AddingMapper_ForSameTypeTwice_ThrowsException()
        {
            var configuration = new EntityMapperConfiguration();
            configuration.AddMapper<ExampleModelWithMapping1, MapperForExampleModel1>();

            Assert.Throws<MapperAlreadyRegisteredForTypeException>(() =>
            {
                configuration.AddMapper<ExampleModelWithMapping1, MapperForExampleModel1>();
            });
        }
    }
}