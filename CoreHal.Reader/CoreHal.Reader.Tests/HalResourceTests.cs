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
    public class HalResourceTests
    {
        [Fact]
        public void Constructing_WithNullResourceLoaderProvided_ThrowsException()
        {
            IHalResponseLoader loader = null;
            var mapperFactory = new Mock<IEntityMapperFactory>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                new HalResource(loader);
            });
        }

        [Fact]
        public void Constructing_WithNullResourceLoaderAndMapperFactoryProvided_ThrowsException()
        {
            IHalResponseLoader loader = null;
            var mapperFactory = new Mock<IEntityMapperFactory>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                new HalResource(loader, mapperFactory.Object);
            });
        }

        [Fact]
        public void Constructing_WithResourceLoaderAndNullMapperFactoryProvided_ThrowsException()
        {
            var loader = new Mock<IHalResponseLoader>();
            IEntityMapperFactory mapperFactory = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                new HalResource(loader.Object, mapperFactory);
            });
        }

        [Fact]
        public void Loading_WithNullRawResponseProvided_ThrowsException()
        {
            var loader = new Mock<IHalResponseLoader>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            Assert.Throws<ArgumentNullException>(() =>
            {
                resource.Load(null);
            });
        }

        [Fact]
        public void Loading_WithEmptyRawResponseProvided_ThrowsException()
        {
            var loader = new Mock<IHalResponseLoader>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            Assert.Throws<ArgumentException>(() =>
            {
                resource.Load(string.Empty);
            });
        }

        [Fact]
        public void Loading_CallsLoad()
        {
            var loader = new Mock<IHalResponseLoader>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            loader.Verify(loader => loader.Load(It.IsAny<string>()));
        }

        [Fact]
        public void Loading_CallsLinkLoader()
        {
            var loader = new Mock<IHalResponseLoader>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            loader.Verify(loader => loader.LoadLinks());
        }

        [Fact]
        public void Loading_CallsPropertyLoader()
        {
            var loader = new Mock<IHalResponseLoader>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            loader.Verify(loader => loader.LoadProperties());
        }

        [Fact]
        public void Loading_CallsEmbeddedItemsLoader()
        {
            var loader = new Mock<IHalResponseLoader>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            loader.Verify(loader => loader.LoadEmbeddedItems());
        }

        [Fact]
        public void GettingProperty_WithNullPropertyNameProvided_ThrowsException()
        {
            var loader = new Mock<IHalResponseLoader>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            Assert.Throws<ArgumentNullException>(() =>
            {
                resource.GetSimplePropertyAs<string>(null);
            });
        }

        [Fact]
        public void GettingProperty_WithEmptyPropertyNameProvided_ThrowsException()
        {
            var loader = new Mock<IHalResponseLoader>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            Assert.Throws<ArgumentException>(() =>
            {
                resource.GetSimplePropertyAs<string>(string.Empty);
            });
        }

        [Fact]
        public void GettingStringProperty_WithPropertyAskedForIsString_ReturnsStringValue()
        {
            const string propertyName = "string-property";
            const string propertyValue = "some string";

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.LoadProperties())
                .Returns(new Dictionary<string, object> { { propertyName, propertyValue } });

            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            var retrievedValue = resource.GetSimplePropertyAs<string>(propertyName);

            Assert.IsType<string>(retrievedValue);
            Assert.Equal(expected: propertyValue, actual: retrievedValue);
        }

        [Fact]
        public void GettingStringProperty_WhenCasingDiffers_ThrowsException()
        {
            const string propertyNameWithCamelCasing = "string-Property";

            const string propertyName = "string-property";
            const string propertyValue = "some string";

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.LoadProperties())
                .Returns(new Dictionary<string, object> { { propertyName, propertyValue } });

            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            Assert.Throws<KeyNotFoundException>(() =>
            {
                resource.GetSimplePropertyAs<string>(propertyNameWithCamelCasing);
            });
        }

        [Fact]
        public void GettingIntProperty_WithPropertyAskedForIsString_ThrowsException()
        {
            const string propertyName = "string-property";
            const string propertyValue = "some string";

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.LoadProperties())
                .Returns(new Dictionary<string, object> { { propertyName, propertyValue } });

            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            Assert.Throws<InvalidCastException>(() =>
            {
                resource.GetSimplePropertyAs<int>(propertyName);
            });
        }

        [Fact]
        public void GettingProperty_WhenItDoesNotExist_ThrowsException()
        {
            const string propertyName = "string-property";
            const string propertyValue = "some string";

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.LoadProperties())
                .Returns(new Dictionary<string, object> { { propertyName, propertyValue } });

            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            Assert.Throws<KeyNotFoundException>(() =>
            {
                resource.GetSimplePropertyAs<int>("non existent property");
            });
        }

        [Fact]
        public void GettingComplexProperty_WhenPropertiesAllMatchWhatIsAskedFor_ReturnsComplexValue()
        {
            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.LoadProperties()).Returns(
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
            resource.Load("some non null and non empty string");

            var retrievedValue = resource.GetComplexPropertyAs<ComplexModel>(propertyName);

            Assert.IsType<ComplexModel>(retrievedValue);
            propertyValue.ShouldDeepEqual(retrievedValue);
        }

        [Fact]
        public void GettingComplexProperty_WhenOnePropertyDoesNotMatch_ThrowsException()
        {
            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.LoadProperties()).Returns(
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
            resource.Load("some non null and non empty string");

            Assert.Throws<KeyNotFoundException>(() =>
            {
                resource.GetComplexPropertyAs<ComplexModel>(propertyName);
            });
        }

        [Fact]
        public void GettingComplexProperty_WhenItIsNotADictionary_ThrowsException()
        {
            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.LoadProperties()).Returns(
                new Dictionary<string, object>
                {
                    {
                        propertyName,
                        new ComplexModel
                        {
                            Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                            Name = "My Name",
                            Date = new DateTime(2020, 7, 5)
                        }
                    }
                });

            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load("some non null and non empty string");

            Assert.Throws<InvalidComplexPropertyException>(() =>
            {
                resource.GetComplexPropertyAs<ComplexModel>(propertyName);
            });
        }

        [Fact]
        public void GettingComplexProperty_QuickMappingExpectedPropertyToKeyValue_ReturnsComplexValue()
        {
            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.LoadProperties()).Returns(
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
            resource.Load("some non null and non empty string");

            var retrievedValue =
                resource.GetComplexPropertyAs<ComplexModel>(
                    propertyName,
                    QuickMap.Into<ComplexModel>(model => model.Name, "full-name"));

            Assert.IsType<ComplexModel>(retrievedValue);
            propertyValue.ShouldDeepEqual(retrievedValue);
        }

        [Fact]
        public void GettingComplexProperty_QuickMappingExpectedPropertyToNonExistentKeyValue_ThrowsException()
        {
            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.LoadProperties()).Returns(
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
            resource.Load("some non null and non empty string");

            Assert.Throws<KeyNotFoundException>(() =>
            {
                var retrievedValue =
                    resource.GetComplexPropertyAs<ComplexModel>(
                        propertyName,
                        QuickMap.Into<ComplexModel>(model => model.Name, "name-that=does-not-exist"));
            });
        }

        [Fact]
        public void GettingComplexProperty_QuickMappingIntoNonExistentExpectedProperty_ThrowsException()
        {
            const string propertyName = "complex";

            var propertyValue =
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                };

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.LoadProperties()).Returns(
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
            resource.Load("some non null and non empty string");

            Assert.Throws<KeyNotFoundException>(() =>
            {
                var retrievedValue =
                resource.GetComplexPropertyAs<ComplexModel>(
                    propertyName,
                    new KeyValuePair<string, string> ("NotAPropertyOfComplexModel","full-name"));
            });
        }

        //[Fact]
        //public void GettingAsEntity_WithNoMapperProvided_ThrowsException()
        //{
        //    const string propertyName = "complex";

        //    var propertyValue =
        //        new ComplexModel
        //        {
        //            Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
        //            Name = "My Name",
        //            Date = new DateTime(2020, 7, 5)
        //        };

        //    var loader = new Mock<IHalResourceLoader>();

        //    loader.Setup(loader => loader.LoadProperties()).Returns(
        //        new Dictionary<string, object>
        //        {
        //            {
        //                propertyName,
        //                new Dictionary<string, object>
        //                {
        //                    { "id", Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a") },
        //                    { "full-name", "My Name" },
        //                    { "date", new DateTime(2020,7,5) }
        //                }
        //            }
        //        });

        //    var resource = new HalResource(loader.Object);
        //    resource.Load("some non null and non empty string");

        //    Assert.Throws<KeyNotFoundException>(() =>
        //    {
        //        //var retrievedValue =
        //        //resource.< ComplexModel > (
        //        //    propertyName,
        //        //    new KeyValuePair<string, string>("NotAPropertyOfComplexModel", "full-name"));
        //    });
        //}
    }
}