using PayMents.Orders.Application.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Abstractions;

public interface IOrderService
{
    Task<OrderDto> Create(CreateOrderDto createOrderDto);
    Task<OrderDto> GetById(int orderId);
    Task<List<OrderDto>> GetByUser(int customerId);
    Task<List<OrderDto>> GetAll();
    Task Reject(int orderId);

}
