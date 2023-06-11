using Assessment.Core.Domain.Models;
using Assessment.Core.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IRepository<Product> _productRepository;

        public ProductController(ILogger<ProductController> logger, IRepository<Product> productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching Product ID: {0}", id);
            try
            {
                var product = await _productRepository.GetByIdAsync(id, cancellationToken);

                if (product is null)
                    return NotFound();


                return Ok(product);
            }

            catch (Exception ex)
            {

                _logger.LogInformation("Unable to fetch Product ID: {0},{1}", id, ex);

                throw;
            }
        }
    }
}