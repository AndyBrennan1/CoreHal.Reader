using System;

namespace CoreHal.Reader.Mapping.Exceptions
{
    public class NoSuchPropertyException : Exception
    {
        private const string message = "No property called ''";
        public string MissingProperty { get; set; }

        public NoSuchPropertyException(string missingProperty) 
            : base($"No property called '{missingProperty}' was found.")
        {
            MissingProperty = missingProperty;
        }
    }
}
