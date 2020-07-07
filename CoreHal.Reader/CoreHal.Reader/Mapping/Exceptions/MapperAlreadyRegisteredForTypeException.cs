using System;

namespace CoreHal.Reader.Mapping.Exceptions
{
    public class MapperAlreadyRegisteredForTypeException : Exception
    {
        public Type TypeAlreadyRegistered { get; set; }

        public MapperAlreadyRegisteredForTypeException(Type typeAlreadyRegistered) 
            : base($"The type {typeAlreadyRegistered.Name} has a mapper registered already.")
        {
            TypeAlreadyRegistered = typeAlreadyRegistered;
        }
    }
}