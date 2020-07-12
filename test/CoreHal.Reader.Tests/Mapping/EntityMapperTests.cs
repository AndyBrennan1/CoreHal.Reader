using CoreHal.Reader.Mapping.Exceptions;
using CoreHal.Reader.Tests.Mapping.Fixtures.Mappers;
using CoreHal.Reader.Tests.Mapping.Fixtures.Models;
using DeepEqual.Syntax;
using System;
using System.Collections.Generic;
using Xunit;

namespace CoreHal.Reader.Tests.Mapping
{
    public class EntityMapperTests
    {
        [Fact]
        public void Constructing_CreatesDefaultEntity()
        {
            var mapper = new MapperWithNoConfiguration();

            Assert.NotNull(mapper.Entity);
        }

        [Fact]
        public void LoadingData_WithNullObjectConfigured_ThrowsException()
        {
            var mapper = new MapperWithNoConfiguration();

            IDictionary<string, object> rawData = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                mapper.LoadData(rawData);
            });
        }

        [Fact]
        public void LoadingData_WithEmptyDictionaryConfigured_ThrowsException()
        {
            var mapper = new MapperWithNoConfiguration();

            var rawData = new Dictionary<string, object>();

            Assert.Throws<ArgumentException>(() =>
            {
                mapper.LoadData(rawData);
            });
        }

        [Fact]
        public void LoadingData_WithDictionaryConfigured_DoesNotThrowException()
        {
            var mapper = new MapperWithNoConfiguration();

            var rawData = new Dictionary<string, object>
            {
                { "some-Integer", 123 }
            };

            mapper.LoadData(rawData);
        }

        [Fact]
        public void Mapping_WithNullRawPropertyNameConfigured_ThrowsException()
        {
            var mapper = new MapperThatProvidesNullPropertyName();

            var rawData = new Dictionary<string, object>
            {
                { "integer-Property", 123 }
            };

            mapper.LoadData(rawData);

            Assert.Throws<ArgumentNullException>(() =>
            {
                mapper.Map();
            });
        }

        [Fact]
        public void Mapping_WithEmptyRawPropertyNameConfigured_ThrowsException()
        {
            var mapper = new MapperThatProvidesEmptyPropertyName();

            var rawData = new Dictionary<string, object>
            {
                { "integer-Property", 123 }
            };

            mapper.LoadData(rawData);

            Assert.Throws<ArgumentException>(() =>
            {
                mapper.Map();
            });
        }

        [Fact]
        public void Mapping_WithUnknownRawDataPropertyNameConfigured_ThrowsException()
        {
            var mapper = new MapperWithUnknownRawDataProperty();

            var rawData = new Dictionary<string, object>
            {
                { "integer-Property", 123 }
            };

            mapper.LoadData(rawData);

            Assert.Throws<KeyNotFoundException>(() =>
            {
                mapper.Map();
            });
        }

        [Fact]
        public void Mapping_WithRequestedPropertyBeingOfDifferentTypeToRequested_ThrowsException()
        {
            var mapper = new MapperSpecifiesWrongTypeForRawDataProperty();

            var rawData = new Dictionary<string, object>
            {
                { "integer-Property", 123 }
            };

            mapper.LoadData(rawData);

            Assert.Throws<InvalidCastException>(() =>
            {
                mapper.Map();
            });
        }

        [Fact]
        public void Mapping_WithRequestedPropertyBeingComplex_ThrowsException()
        {
            var mapper = new MapperMapsComplexPropertyAsSimpleOne();

            var rawData = new Dictionary<string, object>
            {
                {
                    "complex-Property",
                    new Dictionary<string, object>
                    {
                        { "IntegerProperty", 123 },
                        { "StringProperty", "ABC" }
                    }
                }
            };

            mapper.LoadData(rawData);

            Assert.Throws<PropertyIsComplexException>(() =>
            {
                mapper.Map();
            });
        }

        [Fact]
        public void Mapping_WithKnownRawDataPropertyNameConfigured_ReturnMappedValue()
        {
            var mapper = new MapperForExampleModel1();

            var rawData = new Dictionary<string, object>
            {
                { "integer-Property", 123 },
                { "string-Property", "ABC" }
            };

            mapper.LoadData(rawData);

            var result = mapper.Map(); 

            Assert.Equal(expected: 123, actual: result.IntegerProperty);
            Assert.Equal(expected: "ABC", actual: result.StringProperty);
        }

        [Fact]
        public void Mapping_WithComplexPropertyConfiguredWithNullRawPropertyName_ThrowsException()
        {
            var mapper = new MapperThatConfiguresComplexFieldWithNullPropertyName();

            var rawData = new Dictionary<string, object>
            {
                {
                    "complex-Property",
                    new Dictionary<string, object>
                    {
                        { "integer-Property", 123 },
                        { "string-Property", "ABC" }
                    }
                }
            };

            mapper.LoadData(rawData);

            Assert.Throws<ArgumentNullException>(() =>
            {
                mapper.Map();
            });
        }

        [Fact]
        public void Mapping_WithComplexPropertyConfiguredWithEmptyRawPropertyName_ThrowsException()
        {
            var mapper = new MapperThatConfiguresComplexFieldWithEmptyPropertyName();

            var rawData = new Dictionary<string, object>
            {
                {
                    "complex-Property",
                    new Dictionary<string, object>
                    {
                        { "integer-Property", 123 },
                        { "string-Property", "ABC" }
                    }
                }
            };

            mapper.LoadData(rawData);

            Assert.Throws<ArgumentException>(() =>
            {
                mapper.Map();
            });
        }

        [Fact]
        public void Mapping_WithComplexPropertyConfiguredWithUnknownPropertyName_ThrowsException()
        {
            var mapper = new MapperThatConfiguresComplexFieldWithUnknownRawPropertyName();

            var rawData = new Dictionary<string, object>
            {
                {
                    "complex-Property",
                    new Dictionary<string, object>
                    {
                        { "integer-Property", 123 },
                        { "string-Property", "ABC" }
                    }
                }
            };

            mapper.LoadData(rawData);

            Assert.Throws<KeyNotFoundException>(() =>
            {
                mapper.Map();
            });
        }

        [Fact]
        public void Mapping_WithComplexPropertyConfiguredAsBeingSimple_ThrowsException()
        {
            var mapper = new MapperMapsSimplePropertyAsComplexOne();

            var rawData = new Dictionary<string, object>
            {
                { "integer-Property", 123 }
            };

            mapper.LoadData(rawData);

            Assert.Throws<PropertyIsSimpleException>(() =>
            {
                mapper.Map();
            });
        }

        [Fact]
        public void Mapping_ComplexPropertyFullyConfigured_ReturnsMappedValue()
        {
            var mapper = new MapperForExampleModelWithComplexProperty();

            var expectedComplexProperty =
                new ExampleModelWithMapping1 {  
                    IntegerProperty = 123, 
                    StringProperty = "ABC" 
                };

            var rawData = new Dictionary<string, object>
            {
                {
                    "complex-Property",
                    new Dictionary<string, object>
                    {
                        { "integer-Property", 123 },
                        { "string-Property", "ABC" }
                    }
                }
            };

            mapper.LoadData(rawData);

            var result = mapper.Map();

            result.ComplexProperty.ShouldDeepEqual(expectedComplexProperty);
        }
    }
}