﻿using ImageStore.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageStore.Data
{
    public class ImageSeeder
    {
        private readonly ImageContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public ImageSeeder(ImageContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("biljana@shop.com");

            if(user==null)
            {
                user = new StoreUser()
                {
                    FirstName = "Biljana",
                    LastName = "Cvetkoska",
                    Email = "biljana@shop.com",
                    UserName = "biljana@shop.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create a new user in seeder");
                }
            }

            if (!_ctx.Products.Any())
            {
                //create sample data

                var filePath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

                var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }                
            }

            if (!_ctx.Orders.Any())
            {
               
                _ctx.Orders.Add(new Order
                {
                    User = user,
                    OrderDate = DateTime.UtcNow,
                    OrderNumber = "1",
                    Items = new List<OrderItem>()
                        {
                            new OrderItem()
                            {
                                Product = _ctx.Products.First(),
                                Quantity = 5,
                                UnitPrice = _ctx.Products.First().Price
                             }
                        }
                });
            }

            _ctx.SaveChanges();
        }
    }
}
