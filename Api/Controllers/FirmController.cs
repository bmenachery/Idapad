using System.Threading.Tasks;
using Api.Dtos;
using Infrastructure.AppSettings;
using Infrastructure.DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers
{
    public class FirmController: BaseApiController
    {
        private readonly IdapadDataAccess _dataAccess;
        public FirmController(IOptions<ConnectionStrings> connectionStrings)
        {
            _dataAccess = new IdapadDataAccess(connectionStrings.Value.IdapadDb);
        }

        [HttpPost("register")]
        public async Task<ActionResult<int>> RegisterFirm(
                [FromBody] FirmAddressDto model)
        {   
            var firmaddress = new FirmAddress
            {
                Type = (string)model.AddressType,
                AptAddress = (string)model.AptAddress,
                StreetAddress = (string)model.StreetAddress,
                City = (string)model.City,
                State = (string)model.State,
                ZipCode = (string)model.Zip,

            };

          var  addressId = await _dataAccess.CreateFirmAddressAsync(firmaddress);

            var firm = new Firm
            {
                Name = (string)model.Name,
                Type = (string)model.Type,
                AddressId = addressId
            };
           
            var firmId = await _dataAccess.CreateFirmAsync(firm);

            var user = await _dataAccess.GetUserByUserNameAsync(model.userName);

            var firmUser = new FirmUser
            {
                FirmId = (int)firmId,
                UserId = (int)user.Id,
            };

            await _dataAccess.LinkFirmToUserAsync(firmUser);

            return firmId;
            
        }
    }
}