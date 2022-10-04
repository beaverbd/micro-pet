namespace Common.ExternalDataAccess.Interfaces;

public interface IExternalDataStorage<TEntity> where TEntity : ExternalDataItemBase
{
    TEntity? GetById(Guid id);
    void Add(TEntity item);
    void AddRange(IEnumerable<TEntity> items);
    void AddOrUpdate(TEntity item);
    void Delete(Guid id);
    bool Exists(Guid id);
}
