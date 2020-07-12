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
        public void AddingMapper_WithNullMapperProvided_ThrowsException()
        {
            var configuration = new EntityMapperConfiguration();

            Assert.Throws<ArgumentNullException>(() =>
            {
                configuration.AddMapper<ExampleModelWithMapping1>(null);
            });
        }

        [Fact]
        public void AddingMapper_WithMapperProvided_AddsItToTheMapperDictionary()
        {
            var configuration = new EntityMapperConfiguration();

            configuration.AddMapper(new MapperForExampleModel1());

            Assert.Single(configuration.Mappers);

            Assert.IsAssignableFrom<IEntityMapper<ExampleModelWithMapping1>>
                (configuration.Mappers[typeof(ExampleModelWithMapping1)]);
        }

        [Fact]
        public void AddingMapper_ForSameTypeTwice_ThrowsException()
        {
            var configuration = new EntityMapperConfiguration();
            configuration.AddMapper<ExampleModelWithMapping1>(new MapperForExampleModel1());

            Assert.Throws<MapperAlreadyRegisteredForTypeException>(() =>
            {
                configuration.AddMapper<ExampleModelWithMapping1>(new MapperForExampleModel1());
            });
        }
    }
}