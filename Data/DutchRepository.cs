﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Data
{
  public class DutchRepository : IDutchRepository
  {
    private readonly DutchContext _ctx;
    private readonly ILogger<DutchRepository> _logger;

    public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger) //Inject ILogger interface using the class itself
    {
      _ctx = ctx;
      _logger = logger;
    }

    public IEnumerable<Product> GetAllProducts()
    {
      try
      {
        _logger.LogInformation("GetAllProducts was called...");

        return _ctx.Products
                   .OrderBy(p => p.Title)
                   .ToList();

      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to get all products: {ex}"); //catch error and you can log it to a database
        return null;
      }
    }

    public IEnumerable<Order> GetAllOrders(bool includeItems)
    {
      if (includeItems)
      {
        return _ctx.Orders
          .Include(o => o.Items) //include deep hierarchy
          .ThenInclude(i => i.Product) //include a deeper hierarchy
          .ToList();
      }
      else
      {
        return _ctx.Orders
          .ToList();
      }
    }

    public Order GetOrderById(string username, int id)
    {
      return _ctx.Orders
        .Include(o => o.Items)
        .ThenInclude(i => i.Product)
        .Where(o => o.Id == id && o.User.UserName == username) //Get that particular item that belongs to that username
        .FirstOrDefault(); //get a single Order not a List
    }

    public IEnumerable<Product> GetProductsByCategory(string category)
    {
      return _ctx.Products
                 .Where(p => p.Category == category)
                 .ToList();
    }

    public bool SaveAll()
    {
      return _ctx.SaveChanges() > 0;
    }

    public void AddEntity(object entity)
    {
      _ctx.Add(entity);
    }

    public IEnumerable<Order> GetOrdersByUser(string username, bool includeItems)
    {
      if (includeItems) 
      {
        return _ctx.Orders
          .Include(o => o.Items) //if "includeItems" parameter is true return Items
          .ThenInclude(i => i.Product)
          .Where(o => o.User.UserName == username) //get orders in the context of the logged username
          .ToList();
      }
      else //if "includeItems" parameter is false
      {
        return _ctx.Orders
          .Where(o => o.User.UserName == username)
          .ToList();
      }
    }
  }
}
