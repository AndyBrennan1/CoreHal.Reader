using System;

namespace CoreHal.Reader.Mapping.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class EmbeddedItemIsCollectionException : Exception
    {
        public string Key { get; set; }

        public EmbeddedItemIsCollectionException(string key)
        {
            Key = key;
        }
    }
}