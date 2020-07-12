using CoreHal.Graph;
using CoreHal.Reader.Mapping;
using CoreHal.Reader.Mapping.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Validation;

namespace CoreHal.Reader
{
    public class HalResource : IHalResource
    {
        private readonly IHalResponseLoader halResourceLoader;
        private readonly IEntityMapperFactory mapperFactory;

        public IDictionary<string, IEnumerable<Link>> Links { get; private set; }
        public IDictionary<string, object> Properties { get; private set; }
        public IDictionary<string, IEnumerable<HalResource>> EmbeddedItems { get; private set; }

        public HalResource(IHalResponseLoader HalResourceLoader)
        {
            Requires.NotNull(HalResourceLoader, nameof(HalResourceLoader));

            halResourceLoader = HalResourceLoader;
        }

        public HalResource(IHalResponseLoader HalResourceLoader, IEntityMapperFactory mapperFactory)
        {
            Requires.NotNull(HalResourceLoader, nameof(HalResourceLoader));
            Requires.NotNull(mapperFactory, nameof(mapperFactory));

            halResourceLoader = HalResourceLoader;
            this.mapperFactory = mapperFactory;
        }

        public void Load(string rawResponse)
        {
            Requires.NotNullOrEmpty(rawResponse, nameof(rawResponse));

            halResourceLoader.Load(rawResponse);
            Links = halResourceLoader.LoadLinks();
            Properties = halResourceLoader.LoadProperties();
            EmbeddedItems = halResourceLoader.LoadEmbeddedItems();
        }

        public TEntity GetSimplePropertyAs<TEntity>(string propertyName)
        {
            Requires.NotNullOrEmpty(propertyName, nameof(propertyName));

            var foundItem = Properties[propertyName];
            var result = (TEntity)foundItem;

            return result;
        }

        public TEntity GetComplexPropertyAs<TEntity>(
            string propertyName,
            params KeyValuePair<string, string>[] mappings)
            where TEntity : new()
        {
            Requires.NotNullOrEmpty(propertyName, nameof(propertyName));

            TEntity result;

            var foundItem = Properties[propertyName];

            if (foundItem is IDictionary<string, object> dictionary)
            {
                result = CreateFromDictionary<TEntity>(dictionary, mappings);
            }
            else
            {
                throw new InvalidComplexPropertyException(propertyName);
            }

            return result;
        }

        public TEntity CastAs<TEntity>()
            where TEntity : class, new()
        {
            var mapper = this.mapperFactory.GetMapper<TEntity>();
            mapper.LoadData(this.Properties);

            return mapper.Map();
        }

        public IEnumerable<TProperty> GetEmbeddedSet<TProperty>(string embeddedItemKey)
            where TProperty : class, new()
        {
            throw new NotImplementedException();
        }

        private TEntity CreateFromDictionary<TEntity>(
            IDictionary<string, object> dictionary,
            KeyValuePair<string, string>[] mappings)
            where TEntity : new()
        {
            TEntity entity = new TEntity();

            foreach (var propertyInfo in typeof(TEntity).GetProperties())
            {
                string propertyNameToUse;

                if (ThereAreMappingsForThisProperty(mappings, propertyInfo))
                {
                    propertyNameToUse = mappings.First(mapping => mapping.Key == propertyInfo.Name).Value;
                }
                else
                {
                    propertyNameToUse = propertyInfo.Name.ToLower();
                }

                propertyInfo.SetValue(entity, dictionary[propertyNameToUse]);
            }

            return entity;
        }

        private static bool ThereAreMappingsForThisProperty(KeyValuePair<string, string>[] mappings, PropertyInfo propertyInfo)
        {
            return mappings != null && mappings.Any(mapping => mapping.Key == propertyInfo.Name);
        }
    }
}