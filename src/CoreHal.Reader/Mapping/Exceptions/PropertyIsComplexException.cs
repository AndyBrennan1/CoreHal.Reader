using System;

namespace CoreHal.Reader.Mapping.Exceptions
{
    public class PropertyIsComplexException : Exception
    {
        public string PropertyName { get; set; }

        public PropertyIsComplexException(string propertyName) 
            : base($"The property {propertyName} is complex. Try using the GetPropertyAsDictionary method instead." )
        {
            PropertyName = propertyName;
        }
    }
}