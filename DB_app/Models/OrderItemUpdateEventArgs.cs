namespace DB_app.Models;

public class OrderItemUpdateEventArgs
{
    public OrderItem orderItem;
    public int difference;

    public OrderItemUpdateEventArgs(OrderItem orderItem, int difference)
    {
        this.orderItem = this.orderItem;
        this.difference = difference;
    }
}
