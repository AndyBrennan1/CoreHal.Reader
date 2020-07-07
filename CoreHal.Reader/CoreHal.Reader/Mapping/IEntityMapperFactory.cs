namespace CoreHal.Reader.Mapping
{
    public interface IEntityMapperFactory
    {
        void RegisterMappers();
        IEntityMapper<TEntity> GetMapper<TEntity>() where TEntity : class, new();
    }
}