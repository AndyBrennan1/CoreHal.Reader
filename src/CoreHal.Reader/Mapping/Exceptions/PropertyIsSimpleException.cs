using System;

namespace CoreHal.Reader.Mapping.Exceptions
{
    public class PropertyIsSimpleException : Exception
    {
        public string PropertyName { get; set; }

        public PropertyIsSimpleException(string propertyName)
            : base($"The property {propertyName} is simple. Try using the MapTo<TProperty> method instead.")
        {
            PropertyName = propertyName;
        }
    }
}