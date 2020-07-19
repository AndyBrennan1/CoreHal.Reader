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
    public class CastingComplexPropertyAsEntityTests
    {
        [Fact]
        public void CastingComplexPropertyAsEntity_WithNullPropertyNameProvided_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();

            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            Assert.Throws<ArgumentException>(() =>
            {
                resource.CastComplexPropertyTo<ComplexModel>(string.Empty);
            });
        }

        [Fact]
        public void CastingComplexPropertyAsEntity_WithEmptyPropertyNameProvided_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();

            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            Assert.Throws<ArgumentException>(() =>
            {
                resource.CastComplexPropertyTo<ComplexModel>(string.Empty);
            });
        }

        [Fact]
        public void CastingComplexPropertyAsEntity_WhenPropertyDoesNotExist_ThrowsException()
        {
            string validJson = "{ }";

            const string propertyName = "string-property";
            const string propertyValue = "some string";

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson))
                .Returns(new Dictionary<string, object> { { propertyName, propertyValue } });

            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            Assert.Throws<NoSuchPropertyException>(() =>
            {
                resource.CastComplexPropertyTo<ComplexModel>("non existent property");
            });
        }

        [Fact]
        public void CastingComplexPropertyAsEntity_WhenPropertiesAllMatchWhatIsAskedFor_ReturnsComplexValue()
        {
            string validJson = "{ }";

            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.Load(validJson)).Returns(
                new Dictionary<string, object>
                {
                    {
                        propertyName,
                        new Dictionary<string, object>
                        {
                            { "id", Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a") },
                            { "name", "My Name" },
                            { "date", new DateTime(2020,7,5) }
                        }
                    }
                });

            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            var retrievedValue = resource.CastComplexPropertyTo<ComplexModel>(propertyName);

            Assert.IsType<ComplexModel>(retrievedValue);
            propertyValue.ShouldDeepEqual(retrievedValue);
        }

        [Fact]
        public void CastingComplexPropertyAsEntity_WhenOnePropertyDoesNotMatch_ThrowsException()
        {
            string validJson = "{ }";

            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

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

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            Assert.Throws<KeyNotFoundException>(() =>
            {
                resource.CastComplexPropertyTo<ComplexModel>(propertyName);
            });
        }

        [Fact]
        public void CastingComplexPropertyAsEntity_QuickMappingExpectedPropertyToKeyValue_ReturnsComplexValue()
        {
            string validJson = "{ }";

            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

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

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            var retrievedValue =
                resource.CastComplexPropertyTo<ComplexModel>(
                    propertyName,
                    QuickMap.Into<ComplexModel>(model => model.Name, "full-name"));

            Assert.IsType<ComplexModel>(retrievedValue);
            propertyValue.ShouldDeepEqual(retrievedValue);
        }

        [Fact]
        public void CastingComplexPropertyAsEntity_QuickMappingExpectedPropertyToNonExistentKeyValue_ThrowsException()
        {
            string validJson = "{ }";

            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

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

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            Assert.Throws<KeyNotFoundException>(() =>
            {
                var retrievedValue =
                    resource.CastComplexPropertyTo<ComplexModel>(
                        propertyName,
                        QuickMap.Into<ComplexModel>(model => model.Name, "name-that=does-not-exist"));
            });
        }

        [Fact]
        public void CastingComplexPropertyAsEntity_QuickMappingIntoNonExistentExpectedProperty_ThrowsException()
        {
            string validJson = "{ }";

            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

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

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            Assert.Throws<KeyNotFoundException>(() =>
            {
                var retrievedValue =
                resource.CastComplexPropertyTo<ComplexModel>(
                    propertyName,
                    new KeyValuePair<string, string>("NotAPropertyOfComplexModel", "full-name"));
            });
        }

    }
}
