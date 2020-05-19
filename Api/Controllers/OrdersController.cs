using System.Threading.Tasks;
using Api.Dtos;
using Api.Extensions;
using API.Errors;
using AutoMapper;
using Infrastructure.AppSettings;
using Infrastructure.DataAccess;
using Infrastructure.Identity.Models;
using Infrastructure.Models;
using Infrastructure.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers
{
    [Authorize]
    public class OrdersController: BaseApiController
    {
        private readonly IMapper _mapper;

        private readonly IdapadDataAccess _dataAccess;

        private readonly RedisDataAccess _dataRedisAccess;

        public OrdersController(IMapper mapper, IOptions<ConnectionStrings> connectionStrings, IOptions<RedisConnectionStrings>redisConnectionStrings)
        {
            _mapper = mapper;
            _dataAccess = new IdapadDataAccess(connectionStrings.Value.IdapadDb);
            _dataRedisAccess = new RedisDataAccess(redisConnectionStrings.Value.Redis);
        }

        [HttpPost]
        public async Task<IActionResult>CreateOrder(OrderDto orderDto)
        
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var address = _mapper.Map<AddressDto, FirmAddress>(orderDto.ShipToAddress);

            var addressid = await _dataAccess.CreateFirmAddressAsync(address);

            var order = _mapper.Map<OrderDto, Order>(orderDto);

            int orderId = await _dataAccess.CreateOrderAsync(_dataRedisAccess, order, email, orderDto.DeliveryMethodId, addressid);

            if (orderId == 0) return BadRequest(new ApiResponse(400, "Problem Creating order"));

            return Ok();           

        }

        [HttpGet]
        
        public async Task<ActionResult<Order>> GetOrders()
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
        
            var order = await _dataAccess.GetOrderAsync(email);

            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var order = await _dataAccess.GetOrderByIdAsync(email, id);

            if (order == null) return NotFound(new ApiResponse(404));

            return Ok(order);

        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<Order>> GetDeliveryMethods()
        {
            return Ok(await _dataAccess.GetDeliveryMethodsAsync());
        }
    }
    
}