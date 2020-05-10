using ImageStore.Data.Entities;
using System.Collections.Generic;

namespace ImageStore.Data
{
    public interface IImageRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);   
        
        Order GetOrderById(string username, int id);
        IEnumerable<Order> GetAllOrders(bool includeItems);

        bool SaveAll();
        void AddEntity(object model);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
        void AddOrder(Order newOrder);
    }
}