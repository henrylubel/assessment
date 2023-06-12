using Assessment.Core.Domain.Models;
using Assessment.Core.Domain.Repositories;
using Assessment.Core.Dtos;
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
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto, CancellationToken cancellationToken)
        {

            if (orderDto is null || orderDto.Quantity < 1 ||
                (await _productRepository.GetByIdAsync(orderDto.ProductId, cancellationToken)) is null)

                return BadRequest();

            _logger.LogInformation("Creating Order for customer: {0}", orderDto.CustomerId);

            try
            {
                var order = new Order
                {
                    CustomerId = orderDto.CustomerId,
                    ProductId = orderDto.ProductId,
                    Quantity = orderDto.Quantity
                };

                await _orderRepository.CreateAsync(order, cancellationToken);

                return Ok();
            }

            catch (Exception ex)
            {
                _logger.LogInformation("Unable to create Order for customer {0}. {1}", orderDto.CustomerId, ex);

                throw;
            }
        }
    }
}