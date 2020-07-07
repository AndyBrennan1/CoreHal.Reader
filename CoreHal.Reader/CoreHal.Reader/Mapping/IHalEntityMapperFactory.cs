namespace CoreHal.Reader.Mapping
{
    public interface IHalEntityMapperFactory
    {
        void RegisterMappers();
        IHalEntityMapper<TEntity> GetMapper<TEntity>() where TEntity : class, new();
    }
}