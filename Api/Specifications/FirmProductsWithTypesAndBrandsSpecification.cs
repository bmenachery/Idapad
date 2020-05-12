using System;
using System.Linq.Expressions;
using Infrastructure.Models;

namespace Api.Specifications
{
    public class FirmProductsWithTypesAndBrandsSpecification: BaseSpecification<FirmProduct>
    {
        public FirmProductsWithTypesAndBrandsSpecification(FirmProductSpecParams firmProductParams)
            : base(x =>
                     (string.IsNullOrEmpty(firmProductParams.Search) || x.ProductName.ToLower()
                         .Contains(firmProductParams.Search)) &&
                     (!firmProductParams.BrandId.HasValue || x.ProductBrandId == firmProductParams.BrandId) &&
                     (!firmProductParams.TypeId.HasValue || x.ProductTypeId == firmProductParams.TypeId) 
                     
                )

        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.FirmId);
            AddOrderBy(x => x.ProductName);
            ApplyPaging(firmProductParams.PageSize * (firmProductParams.PageIndex - 1), firmProductParams.PageSize);

            if (!string.IsNullOrEmpty(firmProductParams.Sort))
                switch (firmProductParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDecending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.ProductName);
                        break;
                }
        }

        public FirmProductsWithTypesAndBrandsSpecification(int id)
            : base(x => x.FirmId == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }

        public FirmProductsWithTypesAndBrandsSpecification(Expression<Func<FirmProduct, bool>> criteria) : base(criteria)
        {
        }
    }
}