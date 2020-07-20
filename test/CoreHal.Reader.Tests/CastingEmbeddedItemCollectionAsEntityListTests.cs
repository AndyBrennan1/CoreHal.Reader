using CoreHal.Reader.Loading;
using CoreHal.Reader.Mapping;
using CoreHal.Reader.Mapping.Exceptions;
using CoreHal.Reader.Tests.Fixtures;
using CoreHal.Reader.Tests.Mapping.Fixtures.Mappers;
using DeepEqual.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CoreHal.Reader.Tests
{
    public class CastingEmbeddedItemCollectionAsEntityListTests
    {
        [Fact]
        public void CastEmbeddedItemCollectionAsEntityList_WithNullKeyProvided_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();
            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<ArgumentNullException>(() =>
            {
                var retrievedValue = resource.CastEmbeddedCollectionAs<ComplexModel>(null);
            });
        }

        [Fact]
        public void CastEmbeddedItemCollectionAsEntityList_WithEmptyKeyProvided_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();
            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<ArgumentException>(() =>
            {
                var retrievedValue = resource.CastEmbeddedCollectionAs<ComplexModel>(string.Empty);
            });
        }

        [Fact]
        public void CastEmbeddedItemCollectionAsEntityList_WithKeyProvidedForNonExistentEmbeddedItem_ThrowsException()
        {
            string validJson = "{ }";

            var loader = new Mock<IHalResponseLoader>();
            var mapper = new Mock<IEntityMapper<ComplexModel>>();
            var mapperFactory = new Mock<IEntityMapperFactory>();

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            Assert.Throws<NoEmbeddedItemWithKeyException>(() =>
            {
                var retrievedValue = resource.CastEmbeddedCollectionAs<ComplexModel>("Key Does Not Exist");
            });
        }

        [Fact]
        public void CastEmbeddedItemCollectionAsEntityList_WithNoMapperFactoryProvided_ThrowsException()
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

            var resource = new HalResource(loader.Object);

            resource.Load(validJson);

            Assert.Throws<NoMappersProvidedException>(() =>
            {
                var retrievedValue = resource.CastEmbeddedCollectionAs<ComplexModel>(keyName);
            });
        }

        [Fact]
        public void CastEmbeddedItemCollectionAsEntityList_WithValidMapperForTypeRegistered_ReturnsPopulatedEntityList()
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
                                        { "full-Name", "My Name" },
                                        { "date", new DateTime(2020,7,5) }
                                    },
                                    new Dictionary<string, object>
                                    {
                                        { "id", Guid.Parse("04463c48-2802-4c8a-82bb-2a106f52464d") },
                                        { "full-Name", "Your Name" },
                                        { "date", new DateTime(2020,7,20) }
                                    }
                                }
                            }
                        }
                    }
                });

            var expectedEmbeddedModelList = new List<ComplexModel>
            {
                new ComplexModel
                {
                    Id = Guid.Parse("3b947c60-dcf8-4199-95a1-7a70fd81d16a"),
                    Name = "My Name",
                    Date = new DateTime(2020, 7, 5)
                },
                new ComplexModel
                {
                    Id = Guid.Parse("04463c48-2802-4c8a-82bb-2a106f52464d"),
                    Name = "Your Name",
                    Date = new DateTime(2020, 7, 20)
                }
            };

            var mapperFactory = new Mock<IEntityMapperFactory>();

            mapperFactory
                .Setup(factory => factory.GetMapper<ComplexModel>())
                .Returns(() => new MapperForComplexModel());

            var resource = new HalResource(loader.Object, mapperFactory.Object);

            resource.Load(validJson);

            var retrievedValue = resource.CastEmbeddedCollectionAs<ComplexModel>(keyName);

            retrievedValue.ShouldDeepEqual(expectedEmbeddedModelList);
        }
    }
}