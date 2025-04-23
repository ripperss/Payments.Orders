
namespace PayMent.Orders.Domain.Exception;

public class EntityNotFoundException : System.Exception
{
    public EntityNotFoundException(string message) : base(message)
    {
    }
} 