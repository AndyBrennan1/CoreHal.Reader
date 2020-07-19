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
    public class CastingEmbeddedItemAsEntityTests
    {
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