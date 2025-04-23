

using PayMent.Orders.Domain.Models;

namespace PayMent.Orders.Domain.Items;

public class RefreshToken : BaseItem
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public string UserId { get; set; }
}
