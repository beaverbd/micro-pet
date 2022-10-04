using System.Collections.Concurrent;
using Common.ExternalDataAccess.Interfaces;

namespace Common.ExternalDataAccess;

public class ExternalDataStorage<TEntity> : IExternalDataStorage<TEntity> where TEntity : ExternalDataItemBase
{
    private ConcurrentDictionary<Guid, TEntity> _items;

    public ExternalDataStorage()
    {
        _items = new ConcurrentDictionary<Guid, TEntity>();
    }

    public TEntity? GetById(Guid id)
    {
        return _items.TryGetValue(id, out var item)
            ? item
            : null;
    }

    public void Add(TEntity item)
    {
        if (!_items.TryAdd(item.Id, item))
            throw new Exception("Cannot add item");
    }

    public void AddRange(IEnumerable<TEntity> items)
    {
        foreach (var externalDataItemBase in items)
        {
            Add(externalDataItemBase);
        }
    }

    public void AddOrUpdate(TEntity item)
    {
        _items.AddOrUpdate(item.Id, item, (_, _) => item);
    }

    public void Delete(Guid id)
    {
        if (!_items.TryRemove(id, out _))
            throw new Exception("Cannot delete item");
    }

    public bool Exists(Guid id)
    {
        return _items.ContainsKey(id);
    }
}
