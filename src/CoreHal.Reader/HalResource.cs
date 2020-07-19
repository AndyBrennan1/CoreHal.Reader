using CoreHal.Graph;
using CoreHal.Reader.Loading;
using CoreHal.Reader.Loading.Exceptions;
using CoreHal.Reader.Mapping;
using CoreHal.Reader.Mapping.Exceptions;
using System;
using System.Collections;
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
        private readonly LinkDataProcessor linkProcessor = new LinkDataProcessor();
        private readonly EmbeddedItemDataProcessor embeddedItemProcessor = new EmbeddedItemDataProcessor();

        public IDictionary<string, IEnumerable<Link>> Links { get; set; }
        public IDictionary<string, object> Properties { get; set; }
        public IDictionary<string, IEnumerable<HalResource>> EmbeddedItems { get; set; }

        public HalResource(IHalResponseLoader HalResourceLoader)
        {
            Requires.NotNull(HalResourceLoader, nameof(HalResourceLoader));

            halResourceLoader = HalResourceLoader;

            Links = new Dictionary<string, IEnumerable<Link>>();
            Properties = new Dictionary<string, object>();
            EmbeddedItems = new Dictionary<string, IEnumerable<HalResource>>();
        }

        public HalResource(IHalResponseLoader HalResourceLoader, IEntityMapperFactory mapperFactory)
        {
            Requires.NotNull(HalResourceLoader, nameof(HalResourceLoader));
            Requires.NotNull(mapperFactory, nameof(mapperFactory));

            halResourceLoader = HalResourceLoader;
            this.mapperFactory = mapperFactory;

            Links = new Dictionary<string, IEnumerable<Link>>();
            Properties = new Dictionary<string, object>();
            EmbeddedItems = new Dictionary<string, IEnumerable<HalResource>>();
        }

        internal HalResource(IDictionary<string, object> properties)
        {
            this.Properties = properties;
            Links = new Dictionary<string, IEnumerable<Link>>();
            EmbeddedItems = new Dictionary<string, IEnumerable<HalResource>>();

            linkProcessor.Process(properties, this.Links);
            embeddedItemProcessor.Process(properties, this.EmbeddedItems);
        }

        public void Load(string rawResponse)
        {
            Requires.NotNull(rawResponse, nameof(rawResponse));

            var dataDictionary = halResourceLoader.Load(rawResponse);

            if(dataDictionary != null && dataDictionary.Any())
            {
                ValidateProvidedDataContainsAuthorisedPropertyTypes(dataDictionary);

                linkProcessor.Process(dataDictionary, this.Links);
                embeddedItemProcessor.Process(dataDictionary, this.EmbeddedItems);
            }

            Properties = dataDictionary;
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

        private static void ValidateProvidedDataContainsAuthorisedPropertyTypes(IDictionary<string, object> dataDictionary)
        {
            foreach (var keyValuePair in dataDictionary)
            {
                var propertyType = keyValuePair.Value.GetType();

                if (!PropertyIsAllowedType(keyValuePair.Value))
                {
                    throw new RawDataInputException();
                }
            }
        }

        private static bool PropertyIsAllowedType(object property)
        {
            return
                property is IDictionary
                || PropertyIsSystemPrimitiveType(property)
                || PropertyIsSystemReferenceTypeOrStruct(property);
        }

        private static bool PropertyIsSystemPrimitiveType(object property)
        {
            return property.GetType().IsPrimitive;
        }

        private static bool PropertyIsSystemReferenceTypeOrStruct(object property)
        {
            var propertyType = property.GetType();

            return
                propertyType == typeof(string)
                || propertyType == typeof(decimal)
                || propertyType == typeof(Guid)
                || propertyType == typeof(DateTime)
                || propertyType == typeof(TimeSpan)
                || propertyType == typeof(Uri);
        }
    }
}