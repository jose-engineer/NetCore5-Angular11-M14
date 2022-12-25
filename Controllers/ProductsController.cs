using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
  [Route("api/[Controller]")]
  [ApiController] //Tell the documentations tools that this is a api controller
  [Produces("application/json")]
  public class ProductsController : ControllerBase //ControllerBase: All derived types are used to serve HTTP API responses
  {
    private readonly IDutchRepository _repository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IDutchRepository repository, ILogger<ProductsController> logger)
    {
      _repository = repository;
      _logger = logger;
    }

    [HttpGet] //Attributes
    [ProducesResponseType(200)] //Validations
    [ProducesResponseType(400)]
    public ActionResult<IEnumerable<Product>> Get()  //Use ActionResult for content negotiation, you can specify type "IEnumerable<Product>" for a well documented public api
    {
      try
      {
        return Ok(_repository.GetAllProducts()); //return status code
      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to get products: {ex}");
        return BadRequest("failed to get products"); //return status code
            }
    }
  }
}
