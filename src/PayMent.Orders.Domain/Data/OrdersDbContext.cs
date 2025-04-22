using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayMent.Orders.Domain.Items;
using PayMent.Orders.Domain.Models;

namespace PayMent.Orders.Domain.Data;

public class OrdersDbContext : IdentityDbContext<UserIdentity, IdentityRoleEntity, long>
{

    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Cartitem> Cartitems { get; set; }
    public DbSet<MerchantEntity> merchantEntities { get; set; }

    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
        {
            Database.Migrate();
        }

    }
}
