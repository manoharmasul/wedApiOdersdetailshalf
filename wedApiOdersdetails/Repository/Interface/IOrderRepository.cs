using wedApiOdersdetails.Models;

namespace wedApiOdersdetails.Repository.Interface
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAllOrders();
        public Task<Order> GetOrderById(int id);
        public Task<int> AddOrder(Order order);
        public Task<int> UpdateOrder(Order order);
    }
}
