using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Errors;

namespace Api.Controllers
{

    public class HerbieController: BaseApiController
    {
        private readonly IdapadDataAccess _dataAccess;

        public HerbieController(IdapadDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText()
        {
            return "secret stuff";
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _dataAccess.GetProductByIdAsync(42);

            var thingToReturn = thing.ToString();

            return Ok();
        }


        [HttpGet("notFound")]
        public ActionResult GetNotFoundResult()
        {
            var thing = _dataAccess.GetProductByIdAsync(42);

            return NotFound(new ApiResponse(404));
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(404));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundResult(int id)
        {
            return Ok();
        }
    }
}