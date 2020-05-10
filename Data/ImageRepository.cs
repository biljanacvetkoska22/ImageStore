using ImageStore.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageStore.Data
{
    public class ImageRepository : IImageRepository
    {
        private readonly ImageContext _ctx;
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(ImageContext ctx, ILogger<ImageRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts was called");
                return _ctx.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to get all products: {ex}");
                return null;
            }
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

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if(includeItems)
            {
                return _ctx.Orders.Include(o => o.Items)
                       .ThenInclude(i => i.Product)
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
            return _ctx.Orders.Include(o => o.Items)
                       .ThenInclude(i => i.Product)
                       .Where(o => o.Id == id && o.User.UserName == username)
                       .FirstOrDefault();
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                       .Where(o => o.User.UserName == username)
                       .Include(o => o.Items)                       
                       .ThenInclude(i => i.Product)
                       .ToList();
            }

            else
            {
                return _ctx.Orders
                       .Where(o => o.User.UserName == username)
                       .ToList();
            }
        }

        public void AddOrder(Order newOrder)
        {
            // convert new products to lookup of product
            foreach (var item in newOrder.Items)
            {
                item.Product = _ctx.Products.Find(item.Product.Id); // it's good if it fails because products need to exist
            }

            AddEntity(newOrder);
        }
    }
}
