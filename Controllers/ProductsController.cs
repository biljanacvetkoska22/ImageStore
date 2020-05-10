using ImageStore.Data;
using ImageStore.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageStore.Controllers
{
    [Route("api/[Controller]")] // name of controller (products)
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase 
    {
        private readonly IImageRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IImageRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                return Ok(_repository.GetAllProducts());
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to get products: {ex}");
                return BadRequest("Bad request");
            }            
        }
    }
}
