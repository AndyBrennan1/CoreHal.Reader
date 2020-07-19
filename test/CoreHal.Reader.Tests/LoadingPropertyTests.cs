using CoreHal.Reader.Loading;
using CoreHal.Reader.Loading.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CoreHal.Reader.Tests
{
    public class LoadingPropertyTests
    {
        [Fact]
        public void LoadingData_WithNullRawResponseProvided_ThrowsException()
        {
            var loader = new Mock<IHalResponseLoader>();

            var halResource = new HalResource(loader.Object);

            Assert.Throws<ArgumentNullException>(() =>
            {
                halResource.Load(null);
            });
        }

        [Fact]
        public void LoadingData_WithEmptyRawResponseProvided_ResultsInEmptyHalResource()
        {
            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(string.Empty)).Returns(new Dictionary<string, object>());

            var halResource = new HalResource(loader.Object);

            halResource.Load(string.Empty);

            Assert.Empty(halResource.Properties);
            Assert.Empty(halResource.Links);
            Assert.Empty(halResource.EmbeddedItems);
        }

        [Fact]
        public void LoadingData_CallsLoadWithProvidedRawJson()
        {
            var jsonString = "{ \"string-Property\" : \"Some value\" }";

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "string-Property", "Hello World!" }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(jsonString)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(jsonString);

            loader.Verify(loader => loader.Load(jsonString));
        }

        [Fact]
        public void LoadingData_WithOneStringProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "string-Property", "Hello World!" }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneDecimalProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            decimal propertyValue = 10.0M;

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneGuidProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            Guid propertyValue = Guid.NewGuid();

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneDateTimeProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            DateTime propertyValue = DateTime.Now;

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneTimeSpanProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            TimeSpan propertyValue = new TimeSpan(1000);

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneUriProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            Uri propertyValue = new Uri("http://www.someurl.com");

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneIntegerProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            int propertyValue = 123;

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneInt16Property_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            Int16 propertyValue = 10;

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneInt32Property_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            Int32 propertyValue = Int32.MaxValue;

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneInt64Property_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            Int64 propertyValue = Int64.MaxValue;

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneFloatProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            float propertyValue = 999.009F;

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneDoubleProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            double propertyValue = 999.009;

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneBooleanProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            bool propertyValue = true;

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneNullableIntegerProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            int? propertyValue = 123;

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { "property", propertyValue }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithOneDecomposedComplexProperty_SetsItToThePropertiesCollection()
        {
            var validJson = "{ }";

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                { 
                    "complex-Property", 
                    new Dictionary<string,object>
                    {
                        { "string-Property1" , "Hello" },
                        { "string-Property2" , "World!" }
                    } 
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.Equal(expected: expectedLoaderData, actual: resource.Properties);
        }

        [Fact]
        public void LoadingData_WithComplexPropertyProvidedAsReferenceType_ThrowsException()
        {
            var validJson = "{ }";

            IDictionary<string, object> expectedLoaderData = new Dictionary<string, object>
            {
                {
                    "complex-Property",
                    new Fixtures.ComplexModel
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now,
                        Name = "Some Name"
                    }
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(expectedLoaderData);

            var resource = new HalResource(loader.Object);

            Assert.Throws<RawDataInputException>(() =>
            {
                resource.Load(validJson);
            });
        }

    }
}