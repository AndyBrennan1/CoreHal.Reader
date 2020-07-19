using CoreHal.Reader.Loading;
using CoreHal.Reader.Mapping;
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
        public void GettingSimpleProperty_WhenItDoesNotExist_ThrowsException()
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

            Assert.Throws<KeyNotFoundException>(() =>
            {
                resource.CastSimplePropertyTo<int>("non existent property");
            });
        }

        [Fact]
        public void GettingProperty_WithNullPropertyNameProvided_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            Assert.Throws<ArgumentNullException>(() =>
            {
                resource.CastSimplePropertyTo<string>(null);
            });
        }

        [Fact]
        public void GettingProperty_WithEmptyPropertyNameProvided_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            Assert.Throws<ArgumentException>(() =>
            {
                resource.CastSimplePropertyTo<string>(string.Empty);
            });
        }

        [Fact]
        public void GettingStringProperty_WithPropertyAskedForIsString_ReturnsStringValue()
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

            var retrievedValue = resource.CastSimplePropertyTo<string>(propertyName);

            Assert.IsType<string>(retrievedValue);
            Assert.Equal(expected: propertyValue, actual: retrievedValue);
        }

        [Fact]
        public void GettingStringProperty_WhenCasingDiffers_ThrowsException()
        {
            string validJson = "{ }";

            const string propertyNameWithCamelCasing = "string-Property";

            const string propertyName = "string-property";
            const string propertyValue = "some string";

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson))
                .Returns(new Dictionary<string, object> { { propertyName, propertyValue } });

            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);
            resource.Load(validJson);

            Assert.Throws<KeyNotFoundException>(() =>
            {
                resource.CastSimplePropertyTo<string>(propertyNameWithCamelCasing);
            });
        }

        [Fact]
        public void GettingIntProperty_WithPropertyAskedForIsString_ThrowsException()
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

            Assert.Throws<InvalidCastException>(() =>
            {
                resource.CastSimplePropertyTo<int>(propertyName);
            });
        }
    }
}