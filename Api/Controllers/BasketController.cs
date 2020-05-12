using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.AppSettings;
using Infrastructure.DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Api.Dtos;
using StackExchange.Redis;

namespace Api.Controllers
{
    public class BasketController: BaseApiController
    {
        private readonly RedisDataAccess _dataAccess;

        private readonly IMapper _mapper;
        
        public BasketController(IOptions<RedisConnectionStrings> connectionStrings, IMapper mapper)
        {
            connectionStrings.Value.Redis = "localhost";
            _dataAccess = new RedisDataAccess(connectionStrings.Value.Redis);
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _dataAccess.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));

        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {

            var UpdatedBasket = await _dataAccess.UpdateBasketAsync(basket);

            return Ok(UpdatedBasket);

        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await _dataAccess.DeleteBasketAsync(id);

        }
        
    }
}