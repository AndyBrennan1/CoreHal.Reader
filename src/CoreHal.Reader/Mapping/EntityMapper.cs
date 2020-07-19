using CoreHal.Reader.Mapping.Exceptions;
using System.Collections.Generic;
using Validation;

namespace CoreHal.Reader.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntityMapper<TEntity>
        : IEntityMapper<TEntity>
        where TEntity : class, new()
    {
        private IDictionary<string, object> rawData;

        /// <summary>
        /// 
        /// </summary>
        public TEntity Entity { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityMapper()
        {
            Entity = new TEntity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        public void LoadData(IDictionary<string, object> rawData)
        {
            Requires.NotNullOrEmpty(rawData, nameof(rawData));

            this.rawData = rawData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract TEntity Map();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="rawDataPropertyName"></param>
        /// <returns></returns>
        protected TProperty MapTo<TProperty>(string rawDataPropertyName)
        {
            Requires.NotNullOrEmpty(rawDataPropertyName, nameof(rawDataPropertyName));

            var foundItem = rawData[rawDataPropertyName];

            if (PropertyIsDictionaryOfStringObject(foundItem))
                throw new PropertyIsComplexException(rawDataPropertyName);

            var result = (TProperty)foundItem;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawDataPropertyName"></param>
        /// <returns></returns>
        protected IDictionary<string,object> GetPropertyAsDictionary(string rawDataPropertyName)
        {
            Requires.NotNullOrEmpty(rawDataPropertyName, nameof(rawDataPropertyName));

            var foundItem = rawData[rawDataPropertyName];

            if (!PropertyIsDictionaryOfStringObject(foundItem))
                throw new PropertyIsSimpleException(rawDataPropertyName);

            var result = (IDictionary<string, object>)foundItem;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="rawDataPropertyName"></param>
        /// <returns></returns>
        protected ComplexFieldMapper<TProperty> MapToComplex<TProperty>(string rawDataPropertyName) 
            where TProperty : class, new()
        {
            Requires.NotNullOrEmpty(rawDataPropertyName, nameof(rawDataPropertyName));

            var propertiesAsDictionary = GetPropertyAsDictionary(rawDataPropertyName);

            return new ComplexFieldMapper<TProperty>(propertiesAsDictionary);
        }

        private static bool PropertyIsDictionaryOfStringObject(object foundItem)
        {
            return typeof(IDictionary<string, object>).IsAssignableFrom(foundItem.GetType());
        }
    }
}