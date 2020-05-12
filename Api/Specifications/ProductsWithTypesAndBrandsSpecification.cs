using System;
using System.Linq.Expressions;
using Infrastructure.Models;

namespace Api.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification: BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            : base(x =>
                     (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower()
                         .Contains(productParams.Search)) &&
                     (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                     (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
                )

        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
            AddOrderBy(x => x.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDecending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
        }

        public ProductsWithTypesAndBrandsSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }

        public ProductsWithTypesAndBrandsSpecification(Expression<Func<Product, bool>> criteria) : base(criteria)
        {
        }
    }
}