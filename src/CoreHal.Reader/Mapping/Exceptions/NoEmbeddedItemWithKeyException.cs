using System;

namespace CoreHal.Reader.Mapping.Exceptions
{
    public class NoEmbeddedItemWithKeyException : Exception
    {
        public string Key { get; set; }

        public NoEmbeddedItemWithKeyException(string key) 
            : base($"There are no embedded items with a key of '{key}'.")
        {
            Key = key;
        }
    }
}