using CoreHal.Graph;
using System.Collections.Generic;

namespace CoreHal.Reader
{
    public class ResourceData
    {
        public IDictionary<string, IEnumerable<Link>> Links { get; set; }
        public IDictionary<string, object> Properties { get; set; }
        public IDictionary<string, IEnumerable<ResourceData>> EmbeddedItems { get; set; }

        public ResourceData()
        {
            Links = new Dictionary<string, IEnumerable<Link>>();
            Properties = new Dictionary<string, object>();
            EmbeddedItems = new Dictionary<string, IEnumerable<ResourceData>>();
        }
    }
}
