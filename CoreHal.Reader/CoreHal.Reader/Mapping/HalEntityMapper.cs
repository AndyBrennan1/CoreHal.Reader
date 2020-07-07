using System;
using System.Collections.Generic;
using System.Text;

namespace CoreHal.Reader.Mapping
{
    public abstract class HalEntityMapper<TEntity>
        : IHalEntityMapper<TEntity>
        where TEntity : class, new()
    {
        protected TEntity entity;
        protected IDictionary<string, object> rawData;

        public HalEntityMapper()
        {
            entity = new TEntity();
        }

        public abstract TEntity Map(IDictionary<string, object> raw);

        public TProperty MapTo<TProperty>(string propertyName)
        {
            var foundItem = rawData[propertyName];
            var result = (TProperty)foundItem;

            return result;
        }

        public TProperty MapToComplex<TProperty>(string propertyName)
        {
            return default;
        }
    }
}