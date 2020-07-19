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







        [Fact]
        public void CastEmbeddedItemAsEntity_WithNullKeyProvided_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();
            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<ArgumentNullException>(() =>
            {
                var retrievedValue = resource.CastEmbeddedItemAs<ComplexModel>(null);
            });
        }

        [Fact]
        public void CastEmbeddedItemAsEntity_WithEmptyKeyProvided_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();
            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<ArgumentException>(() =>
            {
                var retrievedValue = resource.CastEmbeddedItemAs<ComplexModel>(string.Empty);
            });
        }

        [Fact]
        public void CastEmbeddedItemAsEntity_WithKeyProvidedForNonExistentEmbeddedItem_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();
            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<NoEmbeddedItemWithKeyException>(() =>
            {
                var retrievedValue = resource.CastEmbeddedItemAs<ComplexModel>("Key Does Not Exist");
            });
        }

        [Fact]
        public void CastEmbeddedItemAsEntity_WithKeyProvidedForEmbeddedItemCollection_ThrowsException()
        {
            string validJson = "{ }";

            const string keyName = "some-Key";

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.Load(validJson)).Returns(
                new Dictionary<string, object>
                {
                    {
                        "_embedded", 
                        new Dictionary<string, object>
                        {
                            {
                                keyName,
                                new List<Dictionary<string, object>>
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "id", Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a") },
                                        { "full-name", "My Name" },
                                        { "date", new DateTime(2020,7,5) }
                                    },
                                    new Dictionary<string, object>
                                    {
                                        { "id", Guid.Parse("43a4d9f2-d4fc-4664-a1af-e6cdb1e42d6b") },
                                        { "full-name", "Your Name" },
                                        { "date", new DateTime(2020,7,19) }
                                    }
                                }                               
                            }                            
                        }
                    }
                });

            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<EmbeddedItemIsCollectionException>(() =>
            {
                var retrievedValue = resource.CastEmbeddedItemAs<ComplexModel>(keyName);
            });
        }

        [Fact]
        public void CastEmbeddedItemAsEntity_WithNoMapperFactoryProvided_ThrowsException()
        {
            string validJson = "{ }";

            const string keyName = "some-Key";

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.Load(validJson)).Returns(
                new Dictionary<string, object>
                {
                    {
                        "_embedded",
                        new Dictionary<string, object>
                        {
                            {
                                keyName,
                                new Dictionary<string, object>
                                {
                                    { "id", Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a") },
                                    { "full-name", "My Name" },
                                    { "date", new DateTime(2020,7,5) }
                                }
                            }
                        }
                    }
                });

            var mapper = new Mock<IEntityMapper<ComplexModel>>();

            var resource = new HalResource(loader.Object);

            resource.Load(validJson);

            Assert.Throws<NoMappersProvidedException>(() =>
            {
                var retrievedValue = resource.CastEmbeddedItemAs<ComplexModel>(keyName);
            });
        }

        [Fact]
        public void CastEmbeddedItemAsEntity_WithNoMapperProvidedForEntityType_ThrowsException()
        {
            string validJson = "{ }";

            const string keyName = "some-Key";

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.Load(validJson)).Returns(
                new Dictionary<string, object>
                {
                    {
                        "_embedded",
                        new Dictionary<string, object>
                        {
                            {
                                keyName,
                                new Dictionary<string, object>
                                {
                                    { "id", Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a") },
                                    { "full-name", "My Name" },
                                    { "date", new DateTime(2020,7,5) }
                                }
                            }
                        }
                    }
                });

            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<TypeHasNoMapperException>(() =>
            {
                var retrievedValue = resource.CastEmbeddedItemAs<ComplexModel>(keyName);
            });
        }

        [Fact]
        public void CastEmbeddedItemAsEntity_WithValidMapperForTypeRegistered_ReturnsPopulatedEntity()
        {
            string validJson = "{ }";

            const string keyName = "some-Key";

            var loader = new Mock<IHalResponseLoader>();

            loader.Setup(loader => loader.Load(validJson)).Returns(
                new Dictionary<string, object>
                {
                    {
                        "_embedded",
                        new Dictionary<string, object>
                        {
                            {
                                keyName,
                                new Dictionary<string, object>
                                {
                                    { "id", Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a") },
                                    { "full-name", "My Name" },
                                    { "date", new DateTime(2020,7,5) }
                                }
                            }
                        }
                    }
                });

            var expectedEmbeddedModel = new ComplexModel
            {
                Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                Name = "My Name",
                Date = new DateTime(2020, 7, 5)
            };

            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            mapper.Setup(mapper => mapper.Map()).Returns(expectedEmbeddedModel);

            var mapperFactory = new Mock<IEntityMapperFactory>();
            mapperFactory.Setup(factory => factory.GetMapper<ComplexModel>()).Returns(mapper.Object);

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);
            
            var retrievedValue = resource.CastEmbeddedItemAs<ComplexModel>(keyName);
            
            retrievedValue.ShouldDeepEqual(expectedEmbeddedModel);
        }














    }
}