using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wedApiOdersdetails.Models;
using wedApiOdersdetails.Repository.Interface;

namespace wedApiOdersdetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return Ok(orders);
        }
        [HttpGet("/id")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderById(id);    
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            try
            {
                var ord = await _orderRepository.AddOrder(order);
                return Ok(ord);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            try
            {
                var od = await _orderRepository.UpdateOrder(order);

                return Ok(od);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

    }
}
