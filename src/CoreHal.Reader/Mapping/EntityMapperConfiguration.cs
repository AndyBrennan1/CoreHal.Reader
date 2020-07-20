using CoreHal.Reader.Mapping.Exceptions;
using System;
using System.Collections.Generic;
using Validation;

namespace CoreHal.Reader.Mapping
{
    public class EntityMapperConfiguration
    {
        public Dictionary<Type, object> Mappers { get; private set; }

        public EntityMapperConfiguration()
        {
            Mappers = new Dictionary<Type, object>();
        }

        public EntityMapperConfiguration AddMapper<TEntity, TMapper>() 
            where TEntity : class, new()
            where TMapper : class, new()
        {
            if (Mappers.ContainsKey(typeof(TEntity)))
                throw new MapperAlreadyRegisteredForTypeException(typeof(TEntity));

            Mappers.Add(typeof(TEntity), typeof(TMapper));

            return this;
        }
    }
}