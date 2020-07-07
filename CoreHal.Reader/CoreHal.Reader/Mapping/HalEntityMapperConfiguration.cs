using CoreHal.Reader.Mapping.Exceptions;
using System;
using System.Collections.Generic;
using Validation;

namespace CoreHal.Reader.Mapping
{
    public class HalEntityMapperConfiguration
    {
        public Dictionary<Type, object> Mappers { get; private set; }

        public HalEntityMapperConfiguration()
        {
            Mappers = new Dictionary<Type, object>();
        }

        public HalEntityMapperConfiguration AddMapper<TEntity>(IHalEntityMapper<TEntity> mapper) 
            where TEntity : class, new()
        {
            Requires.NotNull(mapper, nameof(mapper));

            if (Mappers.ContainsKey(typeof(TEntity)))
                throw new MapperAlreadyRegisteredForTypeException(typeof(TEntity));

            Mappers.Add(typeof(TEntity), mapper);

            return this;
        }
    }
}