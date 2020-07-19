using System;

namespace CoreHal.Reader.Mapping.Exceptions
{
    public class NoMappersProvidedException : Exception
    {
        private const string message = "In order to map the properties of a HalResource to an entity of type T a mapper must be provided.";

        public NoMappersProvidedException() 
            : base(message)
        {
        }
    }
}