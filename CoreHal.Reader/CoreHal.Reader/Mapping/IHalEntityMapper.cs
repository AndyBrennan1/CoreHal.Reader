using System.Collections.Generic;

namespace CoreHal.Reader.Mapping
{
    public interface IHalEntityMapper<TEntity>
        where TEntity : class, new()
    {
        public TEntity Map(IDictionary<string, object> raw);
    }
}