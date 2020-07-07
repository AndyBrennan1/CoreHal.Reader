//using CoreHal.Graph;
//using Moq;
//using System;
//using System.Collections.Generic;

//namespace CoreHal.Reader.Tests.Fixtures
//{
//    public class MockLoader : IHalResourceLoader
//    {
//        public void Load(string rawResponse)
//        {
//            // DO NOTHING >>> THIS IS JUST A STUB
//        }

//        public IDictionary<string, IEnumerable<Link>> LoadLinks()
//        {
//            var links = new Dictionary<string, IEnumerable<Link>>
//            {
//                {
//                    "rel1",
//                    new List<Link>() {
//                        new Link("http://myapi.com/api/orders/123")
//                    }
//                },
//                {
//                    "rel2",
//                    new List<Link>() {
//                        new Link("http://myapi.com/api/products")
//                    }
//                }
//            };

//            return links;
//        }

//        public IDictionary<string, object> LoadProperties()
//        {
//            var properties = new Dictionary<string, object>
//            {
//                { "string-property", "some string" },
//                { "integer-property", 999 },
//                { "bool-property", true },
//                { "guid-property", Guid.Parse("a2e965e6-c2fb-42e9-8d33-c3f5cf64cd60") },
//                { "date-property", new DateTime(2020,7,4) }
//            };

//            return properties;
//        }

//        public IDictionary<string, IEnumerable<HalResource>> LoadEmbeddedItems()
//        {
//            var x = new EmbeddedItem1MockLoader();
//            var x1 = new HalResource(x);
//            x1.Load(It.IsAny<String>());

//            var y = new EmbeddedItem1MockLoader();
//            var y1 = new HalResource(y);
//            y1.Load(It.IsAny<String>());

//            var embeddedItems = new Dictionary<string, IEnumerable<HalResource>>
//            {
//                {
//                    "set1",
//                    new List<HalResource>() { x1 }
//                },
//                {
//                    "set2",
//                    new List<HalResource>() { y1 }
//                }
//            };

//            return embeddedItems;
//        }        
//    }
//}