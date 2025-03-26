using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SecondProject.Models;

namespace SecondProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = new List<Product>()
            {
                new Product { Id = 1,ProductName="Elma" },
                new Product { Id = 2,ProductName="Karpuz" },
                new Product { Id = 3,ProductName="Limon" }
            };

            _logger.LogInformation("GetAllProducts Action Has Been Called.");

            return Ok(products);
        }
    }
}