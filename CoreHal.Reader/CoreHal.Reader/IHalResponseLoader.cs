using CoreHal.Graph;
using System.Collections.Generic;

namespace CoreHal.Reader
{
    public interface IHalResponseLoader
    {
        void Load(string rawResponse);
        IDictionary<string, IEnumerable<Link>> LoadLinks();
        IDictionary<string, object> LoadProperties();
        IDictionary<string, IEnumerable<HalResource>> LoadEmbeddedItems();
    }
}