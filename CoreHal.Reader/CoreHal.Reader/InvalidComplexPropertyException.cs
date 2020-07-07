using System;

namespace CoreHal.Reader
{
    public class InvalidComplexPropertyException : Exception
    {
        public string InvalidPropertyName { get; set; }

        public InvalidComplexPropertyException(string invalidPropertyName) 
            : base($"The Complex property {invalidPropertyName} must be decomposed as Dictionary<string,object> to be valid.")
        {
            InvalidPropertyName = invalidPropertyName;
        }
    }
}