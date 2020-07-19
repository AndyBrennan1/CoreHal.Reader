using CoreHal.Reader.Loading;
using CoreHal.Reader.Loading.Exceptions;
using DeepEqual.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CoreHal.Reader.Tests
{
    public class LoadingEmbeddedItemProcessorTests
    {
        [Fact]
        public void LoadingData_EmbeddedItemCollectionNotDictionaryStringObject_ThrowsException()
        {
            var validJson = "{ }";

            var dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_embedded",
                    new Dictionary<string,string>()
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);

            Assert.Throws<EmbeddedItemDataInWrongFormatException>(() =>
            {
                resource.Load(validJson);
            });
        }

        [Fact]
        public void LoadingData_WithDataContainingNoEmbeddedItems_ResultsInEmptyEmbeddedItemsCollection()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "no embedded items",
                    new Dictionary<string, object>()
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.NotNull(resource.EmbeddedItems);
            Assert.Empty(resource.EmbeddedItems);
        }

        [Fact]
        public void LoadingData_WithDataContainingNoEmbeddedItems_ResultsInNoPropertiesCalledEmbedded()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "no embedded items",
                    new Dictionary<string, object>()
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.False(resource.Properties.ContainsKey("_embedded"));
        }

        [Fact]
        public void LoadingData_WithDataContainingOneSingularEmbeddedItemWithOnlyProperties_ProcessesTheItem()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_embedded",
                    new Dictionary<string, object>
                    {
                        {
                            "some-EmbeddedItem",
                            new Dictionary<string, object>
                            {
                                { "string-Property", "Some String" },
                                { "integer-Property", 123 }
                            }
                        }
                    }
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            HalResource expectedResult;

            expectedResult = 
                CreateMockedHalResource(
                    new Dictionary<string, object>
                    {
                        { "string-Property", "Some String" },
                        { "integer-Property", 123 }
                    });

            Assert.NotNull(resource.EmbeddedItems);
            Assert.Single(resource.EmbeddedItems);
            Assert.Single(resource.EmbeddedItems["some-EmbeddedItem"]);
            expectedResult.ShouldDeepEqual(resource.EmbeddedItems["some-EmbeddedItem"].First());
        }

        [Fact]
        public void LoadingData_WithDataContainingOneSingularEmbeddedItemWithOnlyOneLink_ProcessesTheItem()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_embedded",
                    new Dictionary<string, object>
                    {
                        {
                            "some-EmbeddedItem",
                            new Dictionary<string, object>
                            {
                                { 
                                    "_links",
                                    new Dictionary<string, object>
                                    {
                                        {
                                            "self",
                                            new Dictionary<string, object>
                                            {
                                                { "href", "/api/categories" },
                                                { "title", "Some Title" }
                                            }
                                        }
                                    } 
                                }
                            }
                        }
                    }
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            HalResource expectedResult;

            expectedResult =
                CreateMockedHalResource(
                    new Dictionary<string, object>
                    {
                        { "_links",
                            new Dictionary<string, object>
                            {
                                {
                                    "self",
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/categories" },
                                        { "title", "Some Title" }
                                    }
                                }
                            } 
                        }
                    });

            Assert.NotNull(resource.EmbeddedItems);
            Assert.Single(resource.EmbeddedItems);
            Assert.Single(resource.EmbeddedItems["some-EmbeddedItem"]);
            expectedResult.ShouldDeepEqual(resource.EmbeddedItems["some-EmbeddedItem"].First());
        }

        [Fact]
        public void LoadingData_WithDataContainingOneSingularEmbeddedItemWithOnlyOneEmbeddedItem_ProcessesTheItem()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_embedded",
                    new Dictionary<string, object>
                    {
                        {
                            "some-EmbeddedItem",
                            new Dictionary<string, object>
                            {
                                {
                                    "_embedded",
                                    new Dictionary<string, object>
                                    {
                                        {
                                            "some-EmbeddedItemLevel2",
                                            new Dictionary<string, object>
                                            {
                                                { "guid-Property", Guid.Parse("332c0482-cc78-4cbe-9d18-aac8d26a6471") }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            HalResource expectedResult;

            expectedResult =
                CreateMockedHalResource(
                    new Dictionary<string, object>
                    {
                        {
                            "_embedded",
                            new Dictionary<string, object>
                            {
                                {
                                    "some-EmbeddedItemLevel2",
                                    new Dictionary<string, object>
                                    {
                                        { "guid-Property", Guid.Parse("332c0482-cc78-4cbe-9d18-aac8d26a6471") }
                                    }
                                }
                            }
                        }
                    });

            Assert.NotNull(resource.EmbeddedItems);
            Assert.Single(resource.EmbeddedItems);
            Assert.Single(resource.EmbeddedItems["some-EmbeddedItem"]);
            expectedResult.ShouldDeepEqual(resource.EmbeddedItems["some-EmbeddedItem"].First());
        }

        [Fact]
        public void LoadingData_WithDataContainingOneEmbeddedItemSetWithTwoItems_ProcessesTheItem()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_embedded",
                    new Dictionary<string, object>
                    {
                        {
                            "some-EmbeddedItem",
                            new List<Dictionary<string, object>>
                            {
                                new Dictionary<string, object>
                                {
                                    { "string-Property", "Some String" },
                                    { "integer-Property", 123 }
                                },
                                new Dictionary<string, object>
                                {
                                    { "string-Property", "Some other String" },
                                    { "integer-Property", 999 }
                                }
                            }
                        }
                    }
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            var expectedResult = new List<HalResource>
            {
                CreateMockedHalResource(
                    new Dictionary<string, object>
                    {
                        { "string-Property", "Some String" },
                        { "integer-Property", 123 }
                    }),
                CreateMockedHalResource(
                    new Dictionary<string, object>
                    {
                        { "string-Property", "Some other String" },
                        { "integer-Property", 999 }
                    })
            };               

            Assert.NotNull(resource.EmbeddedItems);
            Assert.Single(resource.EmbeddedItems);
            Assert.True(resource.EmbeddedItems["some-EmbeddedItem"].Count() == 2);
            expectedResult.ShouldDeepEqual(resource.EmbeddedItems["some-EmbeddedItem"]);
        }

        [Fact]
        public void LoadingData_WithDataContainingOneEmbeddedItemSetWithTwoItemsAndOneSingularEmbeddedItem_ProcessesTheItem()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_embedded",
                    new Dictionary<string, object>
                    {
                        {
                            "some-EmbeddedItem",
                            new List<Dictionary<string, object>>
                            {
                                new Dictionary<string, object>
                                {
                                    { "string-Property", "Some String" },
                                    { "integer-Property", 123 }
                                },
                                new Dictionary<string, object>
                                {
                                    { "string-Property", "Some other String" },
                                    { "integer-Property", 999 }
                                }
                            }
                        },
                        {
                            "some-otherEmbeddedItem",
                            new Dictionary<string, object>
                            {
                                { "string-Property1", "Hello" },
                                { "string-Property2", "World!" },
                            }
                        }
                    }
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            var expectedEmbeddedItemSet = new List<HalResource>
            {
                CreateMockedHalResource(
                    new Dictionary<string, object>
                    {
                        { "string-Property", "Some String" },
                        { "integer-Property", 123 }
                    }),
                CreateMockedHalResource(
                    new Dictionary<string, object>
                    {
                        { "string-Property", "Some other String" },
                        { "integer-Property", 999 }
                    })
            };

            var expectedEmbeddedItem =
                CreateMockedHalResource(
                    new Dictionary<string, object>
                    {
                        { "string-Property1", "Hello" },
                        { "string-Property2", "World!" }
                    });

            Assert.NotNull(resource.EmbeddedItems);
            Assert.True(resource.EmbeddedItems.Count == 2);
            Assert.True(resource.EmbeddedItems["some-EmbeddedItem"].Count() == 2);
            Assert.True(resource.EmbeddedItems["some-otherEmbeddedItem"].Count() == 1);
            expectedEmbeddedItemSet.ShouldDeepEqual(resource.EmbeddedItems["some-EmbeddedItem"]);
            expectedEmbeddedItem.ShouldDeepEqual(resource.EmbeddedItems["some-otherEmbeddedItem"].First());
        }

        [Fact]
        public void LoadingData_WithDataContainingEmbeddedData_RemovesEmbeddedItemsFromProperties()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_embedded",
                    new Dictionary<string, object>
                    {
                        {
                            "some-EmbeddedItem",
                            new Dictionary<string, object>
                            {
                                { "string-Property", "Some String" },
                                { "integer-Property", 123 }
                            }
                        }
                    }
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.False(resource.Properties.ContainsKey("_embedded"));
        }

        private static HalResource CreateMockedHalResource(IDictionary<string, object> dataBeingLoaded)
        {
            var validJson = "{ }";

            var expectedloader = new Mock<IHalResponseLoader>();
            
            expectedloader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var halResource = new HalResource(expectedloader.Object);
            halResource.Load(validJson);

            return halResource;
        }
    }
}