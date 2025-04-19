using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PayMent.Orders.Domain.Items;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Service;

public class RoleInitializerService : IRoleInitializerService
{

    private readonly RoleManager<IdentityRoleEntity> _roleManager;
    private readonly ILogger<RoleInitializerService> _logger;

    public RoleInitializerService(RoleManager<IdentityRoleEntity> roleManager, ILogger<RoleInitializerService> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task CreateRoleAsync()
    {
        try
        {
            await InitializeRoles(Role.User, Role.Merchant, Role.Admin);
        }
        catch (System.Exception ex)
        {
            _logger.LogWarning("Ошибка при создание ролей");
        }
    }

    private async Task InitializeRoles(params string[] roles)
    {
        foreach (var roleItem in roles)
        {
            if (await _roleManager.RoleExistsAsync(roleItem))
            {
                continue;
            }

            var userRole = new IdentityRoleEntity()
            {
                Name = roleItem
            };

            await _roleManager.CreateAsync(userRole);
        }
    }
}
