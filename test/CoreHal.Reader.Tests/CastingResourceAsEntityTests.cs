using CoreHal.Reader.Loading;
using CoreHal.Reader.Mapping;
using CoreHal.Reader.Mapping.Exceptions;
using CoreHal.Reader.Tests.Fixtures;
using DeepEqual.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CoreHal.Reader.Tests
{
    public class CastingResourceAsEntityTests
    {

        [Fact]
        public void CastingResourceAsEntity_WithNoMapperFactoryProvided_ThrowsException()
        {
            string validJson = "{ }";

            const string propertyName = "complex";

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.Load(validJson)).Returns(
                new Dictionary<string, object>
                {
                    {
                        propertyName,
                        new Dictionary<string, object>
                        {
                            { "id", Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a") },
                            { "full-name", "My Name" },
                            { "date", new DateTime(2020,7,5) }
                        }
                    }
                });

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Throws<NoMappersProvidedException>(() =>
            {
                var retrievedValue = resource.CastResourceAs<ComplexModel>();
            });
        }

        [Fact]
        public void CastingResourceAsEntity_WithNoMapperForTypeRegistered_ThrowsException()
        {
            string validJson = "{ }";

            const string propertyName = "complex";

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.Load(validJson)).Returns(
                new Dictionary<string, object>
                {
                    {
                        propertyName,
                        new Dictionary<string, object>
                        {
                            { "id", Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a") },
                            { "full-name", "My Name" },
                            { "date", new DateTime(2020,7,5) }
                        }
                    }
                });

            var mapperFactory = new Mock<IEntityMapperFactory>();
            mapperFactory.Setup(factory => factory.GetMapper<ComplexModel>()).Returns(() => null);

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<TypeHasNoMapperException>(() =>
            {
                var retrievedValue = resource.CastResourceAs<ComplexModel>();
            });
        }

        [Fact]
        public void CastingResourceAsEntity_WithValidMapperForTypeRegistered_ReturnsPopulatedEntity()
        {
            string validJson = "{ }";

            const string propertyName = "complex";

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.Load(validJson)).Returns(
                new Dictionary<string, object>
                {
                    {
                        propertyName,
                        new Dictionary<string, object>
                        {
                            { "id", Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a") },
                            { "full-name", "My Name" },
                            { "date", new DateTime(2020,7,5) }
                        }
                    }
                });

            var expectedModel = new ComplexModel
            {
                Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                Name = "My Name",
                Date = new DateTime(2020, 7, 5)
            };

            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            mapper.Setup(mapper => mapper.Map()).Returns(expectedModel);

            var mapperFactory = new Mock<IEntityMapperFactory>();
            mapperFactory.Setup(factory => factory.GetMapper<ComplexModel>()).Returns(mapper.Object);

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            var retrievedValue = resource.CastResourceAs<ComplexModel>();

            retrievedValue.ShouldDeepEqual(expectedModel);
        }

        [Fact]
        public void CastingResourceAsEntity_WithInvalidMapperForTypeUsed_ThrowsException()
        {
            string validJson = "{ }";

            const string propertyName = "complex";

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.Load(validJson)).Returns(
                new Dictionary<string, object>
                {
                    {
                        propertyName,
                        new Dictionary<string, object>
                        {
                            { "id", Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a") },
                            { "full-name", "My Name" },
                            { "date", new DateTime(2020,7,5) }
                        }
                    }
                });

            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            mapper.Setup(mapper => mapper.Map()).Throws(new Exception());

            var mapperFactory = new Mock<IEntityMapperFactory>();
            mapperFactory.Setup(factory => factory.GetMapper<ComplexModel>()).Returns(mapper.Object);

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<ProblemWithMapperException>(() =>
            {
                var retrievedValue = resource.CastResourceAs<ComplexModel>();
            });
        }

        [Fact]
        public void CastingResourceAsEntity_WhenResourceHasNoProperties_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.Load(validJson)).Returns(new Dictionary<string, object>());

            var mapper = new Mock<IEntityMapper<ComplexModel>>();

            var mapperFactory = new Mock<IEntityMapperFactory>();
            mapperFactory.Setup(factory => factory.GetMapper<ComplexModel>()).Returns(mapper.Object);

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<ResourceHasNoPropertiesException>(() =>
            {
                var retrievedValue = resource.CastResourceAs<ComplexModel>();
            });
        }

    }
}
