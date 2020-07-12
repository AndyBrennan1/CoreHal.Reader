using System.Collections.Generic;

namespace CoreHal.Reader.Mapping
{
    public interface IEntityMapper<TEntity>
        where TEntity : class, new()
    {
        void LoadData(IDictionary<string, object> rawData);
        TEntity Map();
    }
}