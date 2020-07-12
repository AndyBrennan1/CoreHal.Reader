using System;

namespace CoreHal.Reader.Mapping.Exceptions
{
    public class TypeHasNoMapperException : Exception
    {
        public Type EntityType { get; set; }

        public TypeHasNoMapperException(Type typeWithNoMapper)
            : base($"The type {typeWithNoMapper.Name} has not had a mapper registered for it.")
        {
            EntityType = typeWithNoMapper;
        }
    }
}