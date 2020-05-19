using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace Infrastructure.DataAccess
{
    public static class ProductDataAccess 
    {
        
        public static async Task<Product> GetProductByIdAsync(this IdapadDataAccess dataAccess, int id)
        {

            string sql = "SELECT  p.[Id], p.[Name], p.[PictureUrl], p.[Price], p.[Description], " +
                         "b.[Name] ProductBrand, t.[Name] ProductType " +
                         "FROM[dbo].[Products] p INNER JOIN ProductBrands b on p.ProductBrandId = b.id "+
                         "INNER JOIN ProductTypes t on p.ProductTypeId = t.id " +
                         "WHERE p.Id = @Id  ";

            var parameters = new {Id = id};

            var product = await dataAccess.QueryFirstOrDefaultAsync<Product>(
                                    sql, parameters, null, (int?)CommandType.StoredProcedure);

            return product ;
        }

        public static async Task<PagedResults<Product>> GetProductByAsync(this IdapadDataAccess dataAccess, 
                int page, int pageSize, string sort, int? brandId, int? typeId, string search)
        {
            string sql ="SELECT  p.[Id], p.[Name], p.[Description], p.[Price], p.[PictureUrl]," + 
                        "b.[Name] ProductBrand, t.[Name] ProductType " +
                        "FROM Products p " +
                        "INNER JOIN ProductBrands b on p.ProductBrandId = b.id " +
                        "INNER JOIN ProductTypes t on p.ProductTypeId = t.id " +
                        "WHERE P.ProductBrandId = isnull(@Brandid, P.ProductBrandId) " +
                        "AND P.ProductTypeId = isnull(@Typeid, P.ProductTypeId) " +
                        "AND p.[Name] like  '%' + isnull(@Search, P.Name) + '%' " +
                        "ORDER BY " +
                        "case when @Sort = 'priceAsc' THEN p.Price END ASC, " +
                        "case when @Sort = 'priceDesc' THEN p.Price END DESC, " +
                        "case when isnull(@Sort, 'name') = 'name' then p.[Name] end ASC, " +
                        "case when @Sort = 'namedesc' then p.[Name] end DESC " +
                        "OFFSET @Offset ROWS " +
                        "FETCH NEXT @PageSize ROWS ONLY";

            string countSql =   "SELECT count(*) " +
                                "FROM Products p " + 
                                "INNER JOIN ProductBrands b on p.ProductBrandId = b.id " +
                                "INNER JOIN ProductTypes t on p.ProductTypeId = t.id " +
                                "WHERE P.ProductBrandId = isnull(@Brandid, P.ProductBrandId) " +
                                "AND P.ProductTypeId = isnull(@Typeid, P.ProductTypeId) " +
                                "AND p.[Name] like  '%' + isnull(@Search, P.Name) + '%'";

            var results = new PagedResults<Product>();

            var TCparameters = new
            {
                BrandId = brandId,
                TypeId = typeId,
                Search = search

            };

            var TotalCount = await dataAccess.QueryFirstOrDefaultAsync<int>(countSql, TCparameters);

            var parameters = new
            {
                Offset = (page - 1) * pageSize,
                PageSize = pageSize,
                Sort = sort,
                BrandId = brandId,
                TypeId = typeId,
                Search = search 
            };

            var products = await dataAccess.QueryAsync<Product>(
                sql, parameters);
                 
             
                results.Items = products;  
                results.TotalCount = TotalCount;
           
            return results;
        }

        public static async Task<IEnumerable<ProductType>> GetProductBrandsAsync(this IdapadDataAccess dataAccess)
        {
            string sql = "SELECT  [Id], [Name] FROM dbo.ProductBrands";

            var productTypes = await dataAccess.QueryAsync<ProductType>(sql);
                                    

            return productTypes;
        }

        public static async Task<IEnumerable<ProductType>> GetProductTypesAsync(this IdapadDataAccess dataAccess)
        {
            string sql = "SELECT  [Id], [Name] FROM dbo.ProductTypes";

            var productTypes = await dataAccess.QueryAsync<ProductType>(sql);

            return productTypes;
        }

        

    }
}