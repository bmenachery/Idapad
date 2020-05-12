using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.AppSettings;
using Infrastructure.DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IdapadDataAccess _dataAccess;

        private readonly IConfiguration _config;

        public AuthController(IOptions<ConnectionStrings> connectionStrings, IConfiguration config)
        {
            _dataAccess = new IdapadDataAccess(connectionStrings.Value.IdapadDb);
            _config = config;
        }

        // POST: api/User
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.UserName = model.UserName.ToLower();

            if (await _dataAccess.UserExists(model.UserName))
                return BadRequest("Username already exists");

            var userToReturn = await _dataAccess.Register(model);
            


            return CreatedAtRoute("GetUser", new { controller = "Users", id = userToReturn.Id }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRegister model)
        {
            var user = await _dataAccess.Login(model);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Surname, user.FirmName), //using FirmId in the Givenname of the ClaimTypes
                new Claim(ClaimTypes.GivenName, user.FirmId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUserName(string username)
        {
            var userToReturn = await _dataAccess.GetFirmUserByUserNameAsync(username);
            return Ok(userToReturn);
        }

    }
}