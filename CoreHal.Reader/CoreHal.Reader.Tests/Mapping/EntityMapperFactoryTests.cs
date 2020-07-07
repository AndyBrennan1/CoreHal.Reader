using CoreHal.Reader.Mapping;
using CoreHal.Reader.Mapping.Exceptions;
using CoreHal.Reader.Tests.Mapping.Fixtures.Factories;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;
using Xunit;

namespace CoreHal.Reader.Tests.Mapping
{
    public class EntityMapperFactoryTests
    {
        [Fact]
        public void RegisteringMappers_WhenNothingSetInConfig_ThrowsException()
        {
            var factory = new HalEntityMapperFactoryWithNothingRegistered();
            
            Assert.Throws<NoMappersRegisteredException>(() =>
            {
                factory.RegisterMappers();
            });
        }

        [Fact]
        public void RegisteringMappers_WithTypeMapperSetTwice_ThrowsException()
        {
            var factory = new HalEntityMapperFactoryWithSameTypeMappedTwice();

            Assert.Throws<MapperAlreadyRegisteredForTypeException>(() =>
            {
                factory.RegisterMappers();
            });
        }

        [Fact]
        public void RegisteringMappers_WhenTwoAreSetInConfig_AddsBothToTheMappersDictionary()
        {
            var factory = new HalEntityMapperFactoryWith2TypesRegistered();
            
            factory.RegisterMappers();

            Assert.Equal(expected: 2, actual: factory.Mappers.Count);
            Assert.NotNull(factory.Mappers[typeof(ExampleModelWithMapping1)]);
            Assert.NotNull(factory.Mappers[typeof(ExampleModelWithMapping2)]);
        }

        [Fact]
        public void GettingMapper_AndNoneAreRegistered_ThrowsException()
        {
            var factory = new HalEntityMapperFactoryWith2TypesRegistered();

            Assert.Throws<NoMappersRegisteredException>(() =>
            {
                factory.GetMapper<ExampleModelWithNoMapping>();
            });
        }

        [Fact]
        public void GettingMapper_ForTypeThatIsNotRegistered_ThrowsException()
        {
            var factory = new HalEntityMapperFactoryWith2TypesRegistered();
            factory.RegisterMappers();

            Assert.Throws<TypeHasNoMapperException>(() =>
            {
                factory.GetMapper<ExampleModelWithNoMapping>();
            });
        }

        [Fact]
        public void GettingMapper_ForTypeThatIsRegistered_ReturnsAppropriateMapper()
        {
            var factory = new HalEntityMapperFactoryWith2TypesRegistered();
            factory.RegisterMappers();

            var mapper = factory.GetMapper<ExampleModelWithMapping1>();

            Assert.NotNull(mapper);
            Assert.IsAssignableFrom<IEntityMapper<ExampleModelWithMapping1>>(mapper);
        }

    }
}