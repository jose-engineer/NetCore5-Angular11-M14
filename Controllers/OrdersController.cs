using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
  [Route("api/[Controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class OrdersController : Controller
  {
    private readonly IDutchRepository _repository;
    private readonly ILogger<OrdersController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<StoreUser> _userManager;

    public OrdersController(IDutchRepository repository, 
      ILogger<OrdersController> logger,
      IMapper mapper,
      UserManager<StoreUser> userManager)
    {
      _repository = repository;
      _logger = logger;
      _mapper = mapper;
      _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Get(bool includeItems = true) //if parameter includeItems not included use true, so it is an optional parameter when using query strings, query strings allows you to change behaviour of yur apis
    {
      try
      {
        var username = User.Identity.Name;

        var results = _repository.GetOrdersByUser(username, includeItems);//pass another parameter "includeItems"

                return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(results)); //get "results" collection (IEnumerable<Order>) and map it to IEnumerable<OrderViewModel> or ICollection or OrderViewModel[]
            }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to return orders: {ex}" );
        return BadRequest($"Failed to return orders"); 
      }
    }

    [HttpGet("{id:int}")] //extend the base url and asks for an id with data type that we expect
    public IActionResult Get(int id)
    {
      try
      {
        var order = _repository.GetOrderById(User.Identity.Name, id);
        if (order != null) return Ok(_mapper.Map<OrderViewModel>(order));
        else return NotFound(); 
      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to return orders: {ex}");
        return BadRequest($"Failed to return orders");
      }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]OrderViewModel model) //Mapping from the body when Post
    {
      try
      {
        if (ModelState.IsValid)  //validate model using OrderViewModel class properties
        {
                    var newOrder = _mapper.Map<Order>(model); //map OrderViewModel to Order, we create ProductId because of this

                    //var newOrder = new Order()
                    //{
                    //    Id = model.OrderId,
                    //    OrderDate = model.OrderDate,   
                    //    OrderNumber = model.OrderNumber
                    //};

                    if (newOrder.OrderDate == DateTime.MinValue) //If OrderDate was not provided
          {
            newOrder.OrderDate = DateTime.Now;
          };

          var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
          newOrder.User = currentUser; //link new order created with the current user

          _repository.AddEntity(newOrder);  //When using ViewModel you Add Order not the ViewModel
          if (_repository.SaveAll())
          {
                        //var vm = new OrderViewModel() { 
                        //    OrderId = newOrder.Id,
                        //    OrderDate = newOrder.OrderDate,
                        //    OrderNumber = newOrder.OrderNumber                            
                        //};
                        //return Created($"/api/orders/{vm.OrderId}", vm);
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<OrderViewModel>(newOrder));//map Order to OrderViewModel
                    }
        }
        else
        {
          return BadRequest(ModelState);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to save new order: {ex}");
      }

      return BadRequest("Failed to save new order.");
    }

  }
}
