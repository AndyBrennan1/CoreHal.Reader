using CoreHal.Graph;
using CoreHal.Reader.Loading;
using CoreHal.Reader.Loading.Exceptions;
using DeepEqual.Syntax;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CoreHal.Reader.Tests
{
    public class LoadingLinkProcessorTests
    {
        [Fact]
        public void LoadingData_LinkCollectionNotDictionaryStringObject_ThrowsException()
        {
            var validJson = "{ }";

            var dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_links",
                    new Dictionary<string,string>()
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);

            Assert.Throws<LinksDataInWrongFormatException>(() =>
            {
                resource.Load(validJson);
            });
        }

        [Fact]
        public void LoadingData_WithDataContainingNoLinks_ResultsInEmptyLinkCollection()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "not links",
                    new Dictionary<string, object>()
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.NotNull(resource.Links);
            Assert.Empty(resource.Links);
        }

        [Fact]
        public void LoadingData_WithDataContainingOneRelWithOneLink_ProcessesLink()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
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
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            var expectedLink = new Link("/api/categories", "Some Title");

            Assert.NotNull(resource.Links);
            Assert.Single(resource.Links);
            Assert.Single(resource.Links["self"]);
            expectedLink.ShouldDeepEqual(resource.Links["self"].First());
        }

        [Fact]
        public void LoadingData_WithDataContainingOneRelWithOneLink_RemovesLinksFromProperties()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
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
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            Assert.False(resource.Properties.ContainsKey("_links"));
        }

        [Fact]
        public void LoadingData_WithDataContainingOneRelWithTwoLinks_ProcessesLinks()
        {
            var validJson = "{ }";

            var dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_links",
                    new Dictionary<string,object>
                    {
                        {
                            "orders",
                            new List<Dictionary<string, object>>
                            {
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/orders/123" },
                                        { "title", "Order" }
                                    }
                                },
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/orders/456" },
                                        { "title", "Order" }
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

            var expectedLinks = new List<Link>
            {
                new Link("/api/orders/123", "Order"),
                new Link("/api/orders/456", "Order")
            };

            var relSet = (List<Link>)resource.Links["orders"];

            Assert.NotNull(resource.Links);
            Assert.Single(resource.Links);
            Assert.True(relSet.Count == 2);
            expectedLinks.ShouldDeepEqual(relSet);
        }

        [Fact]
        public void LoadingData_WithDataContainingTwoRelsEachWithOneLink_ProcessesLinks()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_links",
                    new Dictionary<string, object>
                    {
                        {
                            "self",
                            new Dictionary<string, object>
                            {
                                { "href", "/api/categories/123" },
                                { "title", "Category" }
                            }
                        },
                         {
                            "parent",
                            new Dictionary<string, object>
                            {
                                { "href", "/api/categories" },
                                { "title", "Categories" }
                            }
                        }
                    }
                }
            };

            var loader = new Mock<IHalResponseLoader>();
            loader.Setup(loader => loader.Load(validJson)).Returns(dataBeingLoaded);

            var resource = new HalResource(loader.Object);
            resource.Load(validJson);

            var expectedSelfLink = new Link("/api/categories/123", "Category");
            var expectedParentLink = new Link("/api/categories", "Categories");

            Assert.NotNull(resource.Links);
            Assert.True(resource.Links.Count == 2);
            Assert.Single(resource.Links["self"]);
            Assert.Single(resource.Links["parent"]);
            expectedSelfLink.ShouldDeepEqual(resource.Links["self"].First());
            expectedParentLink.ShouldDeepEqual(resource.Links["parent"].First());
        }

        [Fact]
        public void LoadingData_WithDataContainingTwoRelsEachWithTwoLinks_ProcessesLinks()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
            {
                {
                    "_links",
                    new Dictionary<string, object>
                    {
                        {
                           "orders",
                            new List<Dictionary<string, object>>
                            {
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/orders/123" },
                                        { "title", "Order" }
                                    }
                                },
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/orders/456" },
                                        { "title", "Order" }
                                    }
                                }
                            }
                        },
                        {
                            "products",
                            new List<Dictionary<string, object>>
                            {
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/products/product1" },
                                        { "title", "Product" }
                                    }
                                },
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/products/product2" },
                                        { "title", "Product" }
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

            var expectedOrderLinks = new List<Link>
            {
                new Link("/api/orders/123", "Order"),
                new Link("/api/orders/456", "Order")
            };

            var expectedProductLinks = new List<Link>
            {
                new Link("/api/products/product1", "Product"),
                new Link("/api/products/product2", "Product")
            };

            Assert.NotNull(resource.Links);
            Assert.True(resource.Links.Count == 2);
            Assert.True(resource.Links["orders"].Count() == 2);
            Assert.True(resource.Links["products"].Count() == 2);
            expectedOrderLinks.ShouldDeepEqual(resource.Links["orders"]);
            expectedProductLinks.ShouldDeepEqual(resource.Links["products"]);
        }

        [Fact]
        public void LoadingData_WithDataContainingTwoRelsOneWithTwoLinksTheOtherJustOne_ProcessesLinks()
        {
            var validJson = "{ }";

            IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>
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
                        },
                        {
                            "products",
                            new List<Dictionary<string, object>>
                            {
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/products/product1" },
                                        { "title", "Product" }
                                    }
                                },
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/products/product2" },
                                        { "title", "Product" }
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

            var expectedSelfLink = new Link("/api/categories", "Some Title");

            var expectedProductLinks = new List<Link>
            {
                new Link("/api/products/product1", "Product"),
                new Link("/api/products/product2", "Product")
            };

            Assert.NotNull(resource.Links);
            Assert.True(resource.Links.Count == 2);
            Assert.True(resource.Links["self"].Count() == 1);
            Assert.True(resource.Links["products"].Count() == 2);
            expectedSelfLink.ShouldDeepEqual(resource.Links["self"].First());
            expectedProductLinks.ShouldDeepEqual(resource.Links["products"]);
        }
    }
}