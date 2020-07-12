using CoreHal.Graph;
using System.Collections.Generic;

namespace CoreHal.Reader
{
    public interface IHalResource
    {
        IDictionary<string, IEnumerable<HalResource>> EmbeddedItems { get; }
        IDictionary<string, IEnumerable<Link>> Links { get; }
        IDictionary<string, object> Properties { get; }

        TEntity CastAs<TEntity>() where TEntity : class, new();
        TEntity GetComplexPropertyAs<TEntity>(string propertyName, params KeyValuePair<string, string>[] mappings) where TEntity : new();
        IEnumerable<TProperty> GetEmbeddedSet<TProperty>(string embeddedItemKey) where TProperty : class, new();
        TEntity GetSimplePropertyAs<TEntity>(string propertyName);
        void Load(string rawResponse);
    }
}