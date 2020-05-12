using System.Threading.Tasks;
using Api.Dtos;
using Infrastructure.Models;
using Infrastructure.AppSettings;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;
using System.Collections.Generic;
using Api.Specifications;
using System.Linq;
using Api.Helpers;

namespace Api.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IdapadDataAccess _dataAccess;

        private readonly IMapper _mapper;

        public ProductsController(IOptions<ConnectionStrings> connectionStrings,
                                    IMapper mapper)
        {
            _mapper = mapper;
            _dataAccess = new IdapadDataAccess(connectionStrings.Value.IdapadDb);

        }

        [HttpGet]
        public async Task<ActionResult<ProductToReturnDto>> GetProducts(
             [FromQuery] ProductSpecParams productParams)
        {

            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var results = await _dataAccess.GetProductByAsync(
                                productParams.PageIndex, 
                                productParams.PageSize,
                                productParams.Sort,
                                productParams.BrandId,
                                productParams.TypeId,
                                productParams.Search);

            var totalItems = results.TotalCount;

            var products = results.Items;

            var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);

           

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,
                        productParams.PageSize, totalItems, data));

            
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _dataAccess.GetProductByIdAsync(id);

            var data = _mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(data);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _dataAccess.GetProductBrandsAsync());

        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _dataAccess.GetProductTypesAsync());

        }

        
    }
}