using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Helpers;
using Api.Specifications;
using AutoMapper;
using Infrastructure.AppSettings;
using Infrastructure.DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers
{
    public class FirmProductsController: BaseApiController
    {
        private readonly IdapadDataAccess _dataAccess;

        private readonly IMapper _mapper;

        public FirmProductsController(IOptions<ConnectionStrings> connectionStrings,
                                    IMapper mapper)
        {
            _mapper = mapper;
            _dataAccess = new IdapadDataAccess(connectionStrings.Value.IdapadDb);

        }
        
        [HttpPost("addFirmProduct")]
        public async Task<ActionResult<int>> AddFirmProduct(
                [FromBody] FirmProductDto firmProductDto)
        {
            var firmProduct = new FirmProduct
            {
                FirmId = (int)firmProductDto.FirmId,
                ProductId = (int)firmProductDto.ProductId
            };

            var firmProductId = await _dataAccess.AddFirmProductAsync(firmProduct);
            return Ok(firmProductId);
        }

        [HttpPost("removeFirmProduct")]
        public async Task<ActionResult<bool>> RemoveFirmProduct(
                 [FromBody] FirmProductDto firmProductDto)
        {
            var success = await _dataAccess.RemoveFirmProductAsync(firmProductDto.Id);
            return Ok(success);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FirmProductToReturnDto>> GetFirmProduct(int id)
        {
            var firmproduct = await _dataAccess.GetFirmProductByIdAsync(id);

            var data = _mapper.Map<FirmProduct, FirmProductToReturnDto>(firmproduct);

            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult<FirmProductToReturnDto>> GetFirmProducts(
             [FromQuery] FirmProductSpecParams firmProductParams)
        {

            var spec = new FirmProductsWithTypesAndBrandsSpecification(firmProductParams);

            var results = await _dataAccess.GetFirmProductByAsync(
                                firmProductParams.FirmId,
                                firmProductParams.PageIndex,
                                firmProductParams.PageSize,
                                firmProductParams.Sort,
                                firmProductParams.BrandId,
                                firmProductParams.TypeId,
                                firmProductParams.Search);

            var totalItems = results.TotalCount;

            var firmProducts = results.Items;

            var data = _mapper.Map<IEnumerable<FirmProduct>, IEnumerable<FirmProductToReturnDto>>(firmProducts);



            return Ok(new Pagination<FirmProductToReturnDto>(firmProductParams.PageIndex,
                        firmProductParams.PageSize, totalItems, data));


        }
    }
}