using CoreHal.Reader.Mapping.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreHal.Reader.Mapping
{
    public abstract class EntityMapperFactory : IEntityMapperFactory
    {
        protected EntityMapperConfiguration configuration;

        public Dictionary<Type, object> Mappers { get; set; }

        public IEntityMapper<TEntity> GetMapper<TEntity>() where TEntity : class, new()
        {
            if (Mappers == null)
                throw new NoMappersRegisteredException();

            if (!Mappers.ContainsKey(typeof(TEntity)))
                throw new TypeHasNoMapperException(typeof(TEntity));

            var mapperType = (Type)Mappers[typeof(TEntity)];

            var mapperInstance = (IEntityMapper<TEntity>)Activator.CreateInstance(mapperType);

            return mapperInstance;
        }

        public void RegisterMappers()
        {
            configuration = new EntityMapperConfiguration();
            Configure(configuration);

            if (configuration.Mappers == null || !configuration.Mappers.Any())
                throw new NoMappersRegisteredException();

            Mappers = configuration.Mappers;
        }

        public abstract void Configure(EntityMapperConfiguration cfg);
    }
}