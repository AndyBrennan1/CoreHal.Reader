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
    /// <summary>
    /// 
    /// </summary>
    public class HalResource : IHalResource, IHalResourceLoader
    {
        private readonly IHalResponseLoader halResourceLoader;
        private readonly IEntityMapperFactory mapperFactory;
        private readonly LinkDataProcessor linkProcessor = new LinkDataProcessor();
        private readonly EmbeddedItemDataProcessor embeddedItemProcessor = new EmbeddedItemDataProcessor();

        /// <inheritdoc/>
        public IDictionary<string, IEnumerable<Link>> Links { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, object> Properties { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IEnumerable<HalResource>> EmbeddedItems { get; set; }

        /// <summary>
        /// Instantiates a new instance of <see cref="HalResource"/> with a <see cref="IHalResponseLoader"/> provided
        /// to 'inject' the response data in a format that <see cref="HalResource"/> can understand.
        /// </summary>
        /// <param name="HalResourceLoader">
        /// The <see cref="IHalResponseLoader"/> responsible for 'injecting' data into the <see cref="HalResource"/>
        /// in a way that it understands it.
        /// </param>
        public HalResource(IHalResponseLoader HalResourceLoader)
        {
            Requires.NotNull(HalResourceLoader, nameof(HalResourceLoader));

            halResourceLoader = HalResourceLoader;

            Links = new Dictionary<string, IEnumerable<Link>>();
            Properties = new Dictionary<string, object>();
            EmbeddedItems = new Dictionary<string, IEnumerable<HalResource>>();
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="HalResource"/> with an <see cref="IHalResponseLoader"/> provided
        /// to 'inject' the response data in a format that <see cref="HalResource"/> can understand and an
        /// <see cref="IEntityMapperFactory"/> that supplies data type mappers for casting the data contained within the 
        /// <see cref="HalResource"/> to custom entity types.
        /// </summary>
        /// <param name="HalResourceLoader"></param>
        /// <param name="mapperFactory"></param>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public TEntity CastSimplePropertyTo<TEntity>(string propertyName)
        {
            Requires.NotNullOrEmpty(propertyName, nameof(propertyName));

            var foundItem = Properties[propertyName];
            var result = (TEntity)foundItem;

            return result;
        }

        /// <inheritdoc/>
        public TEntity CastComplexPropertyTo<TEntity>(
            string propertyName,
            params KeyValuePair<string, string>[] mappings)
            where TEntity : new()
        {
            Requires.NotNullOrEmpty(propertyName, nameof(propertyName));

            TEntity result;
            object foundItem;

            if(!Properties.ContainsKey(propertyName))
                throw new NoSuchPropertyException(propertyName);

            foundItem = Properties[propertyName];

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

        /// <inheritdoc/>
        public TEntity CastResourceAs<TEntity>() 
            where TEntity : class, new()
        {
            return CastDictionaryTo<TEntity>(this.Properties);
        }

        /// <inheritdoc/>
        public TEmbedded CastEmbeddedItemAs<TEmbedded>(string embeddedItemKey) 
            where TEmbedded : class, new()
        {
            Requires.NotNullOrEmpty(embeddedItemKey, nameof(embeddedItemKey));

            HalResource embeddedItem;

            if (!EmbeddedItems.ContainsKey(embeddedItemKey))
                throw new NoEmbeddedItemWithKeyException(embeddedItemKey);

            if (EmbeddedItems[embeddedItemKey].Count() > 1)
                throw new EmbeddedItemIsCollectionException(embeddedItemKey);

            embeddedItem = EmbeddedItems[embeddedItemKey].First();

            var embeddedEntity = CastDictionaryTo<TEmbedded>(embeddedItem.Properties);

            return embeddedEntity;
        }

        /// <inheritdoc/>
        public IEnumerable<TProperty> CastEmbeddedItemSetAs<TProperty>(string embeddedItemKey)
            where TProperty : class, new()
        {          
            throw new NotImplementedException();
        }

        private TEntity CastDictionaryTo<TEntity>(IDictionary<string, object> properties) where TEntity : class, new()
        {
            TEntity result;

            if (this.mapperFactory == null)
                throw new NoMappersProvidedException();

            var mapper = this.mapperFactory.GetMapper<TEntity>();

            if (mapper == null)
                throw new TypeHasNoMapperException(typeof(TEntity));

            if (properties == null || !properties.Any())
                throw new ResourceHasNoPropertiesException();

            mapper.LoadData(properties);

            try
            {
                result = mapper.Map();
            }
            catch (Exception exception)
            {
                throw new ProblemWithMapperException(exception);
            }

            return result;
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