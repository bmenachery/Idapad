using System.Threading.Tasks;
using Infrastructure.AppSettings;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IdapadDataAccess _dataAccess;


        public UsersController(IOptions<ConnectionStrings> connectionStrings)
        {
            _dataAccess = new IdapadDataAccess(connectionStrings.Value.IdapadDb);
        }

         // GET api/user/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userToReturn = await _dataAccess.GetUserAsync(id);

            return Ok(userToReturn);
        } 


        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUserName(string username)
        {
            var userToReturn = await _dataAccess.GetFirmUserByUserNameAsync(username);
            return Ok(userToReturn);
        }
       
        
    }
}