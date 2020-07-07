﻿using CoreHal.Graph;
using System;
using System.Collections.Generic;

namespace CoreHal.Reader.Tests.Fixtures
{
    public class EmbeddedItem2MockLoader : IHalResourceLoader
    {
        public void Load(string rawResponse)
        {
            // DO NOTHING >>> THIS IS JUST A STUB
        }

        public IDictionary<string, IEnumerable<HalResource>> LoadEmbeddedItems()
        {
            return null;
        }

        public IDictionary<string, IEnumerable<Link>> LoadLinks()
        {
            var links = new Dictionary<string, IEnumerable<Link>>
            {
                {
                    "something-else",
                    new List<Link>() {
                        new Link("http://myapi.com/api/another-thing")
                    }
                }
            };

            return links;
        }

        public IDictionary<string, object> LoadProperties()
        {
            var properties = new Dictionary<string, object>
            {
                { "id", Guid.Parse("a2e965e6-c2fb-42e9-8d44-c3f5cf64cd60") },
                { "name", "some name" },
                { "description", "some description" }
            };

            return properties;
        }
    }
}