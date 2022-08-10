using Common.Enums;

namespace Orders.App.Entities;

public class Order
{
    public Guid Id { get; set; }
    public OrderStatusType Status { get; set; }
}
