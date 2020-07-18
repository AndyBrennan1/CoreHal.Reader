using CoreHal.Graph;
using CoreHal.Reader.Loading;
using DeepEqual.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CoreHal.Reader.Tests
{
    public class HalResourceDataLoadingTests
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