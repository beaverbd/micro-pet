using Common.Enums;

namespace Common.ExternalDataAccess;

public class ExternalDataItemBase
{
    public Guid Id { get; set; }
}

public class ExternalDataItem<TItem>
{
    public TItem Item { get; set; }
    public DataItemType Type { get; set; }
}
