using Assessment.Core.Domain.Models;
using Assessment.Core.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;

        public OrderController(ILogger<OrderController> logger, IRepository<Order> orderRepository, IRepository<Product> productRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching Order ID: {0}", id);
            try
            {
                var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

                if (order is null)
                    return NotFound();


                return Ok(order);
            }

            catch (Exception ex)
            {

                _logger.LogInformation("Unable to fetch Order ID: {0},{1}", id, ex);

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order, CancellationToken cancellationToken)
        {

            if (order is null || order.Quantity < 1 ||
                (await _productRepository.GetByIdAsync(order.ProductId, cancellationToken)) is null)

                return BadRequest();

            _logger.LogInformation("Creating Order for customer: {0}", order.CustomerId);

            try
            {
                await _orderRepository.CreateAsync(order, cancellationToken);

                return Ok();
            }

            catch (Exception ex)
            {
                _logger.LogInformation("Unable to create Order for customer {0}. {1}", order.CustomerId, ex);

                throw;
            }
        }
    }
}