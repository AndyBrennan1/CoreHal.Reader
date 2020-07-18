using System.Collections.Generic;

namespace CoreHal.Reader.Loading
{
    public interface IHalResponseLoader
    {
        IDictionary<string, object> Load(string rawResponse);
    }
}