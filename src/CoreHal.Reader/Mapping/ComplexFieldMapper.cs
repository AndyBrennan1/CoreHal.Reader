using System;
using System.Collections.Generic;

namespace CoreHal.Reader.Mapping
{
    public class ComplexFieldMapper<TEntity> 
        where TEntity : class, new()
    {
        private readonly IDictionary<string, object> fields;

        public ComplexFieldMapper(IDictionary<string, object> fields)
        {
            this.fields = fields;
        }

        public TEntity Map(Func<IDictionary<string, object>, TEntity> complexItemMapping)
        {
            return complexItemMapping.Invoke(fields);
        }
    }
}