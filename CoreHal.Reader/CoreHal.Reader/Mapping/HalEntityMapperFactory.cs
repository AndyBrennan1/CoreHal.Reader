using CoreHal.Reader.Mapping.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreHal.Reader.Mapping
{
    public abstract class HalEntityMapperFactory : IHalEntityMapperFactory
    {
        protected HalEntityMapperConfiguration configuration;

        public Dictionary<Type, object> Mappers { get; set; }

        public IHalEntityMapper<TEntity> GetMapper<TEntity>() where TEntity : class, new()
        {
            if (Mappers == null)
                throw new NoMappersRegisteredException();

            if (!Mappers.ContainsKey(typeof(TEntity)))
                throw new TypeHasNoMapperException(typeof(TEntity));

            return (IHalEntityMapper<TEntity>)Mappers[typeof(TEntity)];
        }

        public void RegisterMappers()
        {
            configuration = new HalEntityMapperConfiguration();
            Configure(configuration);

            if (configuration.Mappers == null || !configuration.Mappers.Any())
                throw new NoMappersRegisteredException();

            Mappers = configuration.Mappers;
        }

        public abstract void Configure(HalEntityMapperConfiguration cfg);
    }
}