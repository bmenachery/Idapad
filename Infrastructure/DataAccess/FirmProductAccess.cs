using System.Data;
using System.Threading.Tasks;
using Infrastructure.AppSettings;
using Infrastructure.Models;

namespace Infrastructure.DataAccess
{
    public static class FirmProductAccess
    {

        public static async Task<int> AddFirmProductAsync(this IdapadDataAccess dataAccess,
             FirmProduct firmProduct)
        {
            var firmProductId = await DoestheFirmProductExists(dataAccess, firmProduct);

            if(firmProductId < 1)
            {
                return await dataAccess.ExecuteScalarAsync<int>(
                    "Insert FirmProducts (ProductId, FirmId)  output inserted.Id values(@ProductId, @FirmId)", firmProduct);
            }     
                return firmProductId; 


        }

        public static async Task<bool> RemoveFirmProductAsync(this IdapadDataAccess dataAccess,
            int Id)
        {
            
            
            await dataAccess.ExecuteScalarAsync<bool>(
                    "Delete FirmProducts WHERE Id = " + Id);
            return true;


        }

        

        private static async Task<int> DoestheFirmProductExists(this IdapadDataAccess dataAccess,
             FirmProduct firmProduct)
             {
               
               int firmProductId = await dataAccess.QueryFirstOrDefaultAsync<int>(
               "select Id from FirmProducts where ProductId = @ProductId and FirmId = @FirmId", firmProduct);

               return firmProductId;

             }

        public static async Task<FirmProduct> GetFirmProductByIdAsync(this IdapadDataAccess dataAccess, int id)
        {

            string sql = "SELECT  p.[Id], p.[Name], p.[PictureUrl], p.[Price], p.[Description], " +
                         "b.[Name] ProductBrand, t.[Name] ProductType " +
                         "FROM FirmProducts FP " +
                         "INNER JOIN Products p on p.Id = FP.ProductId " + 
                         "INNER JOIN ProductBrands b on p.ProductBrandId = b.id " +
                         "INNER JOIN ProductTypes t on p.ProductTypeId = t.id " +
                         "WHERE FP.Id = @Id  ";

            var parameters = new { Id = id };

            var firmproduct = await dataAccess.QueryFirstOrDefaultAsync<FirmProduct>(
                                    sql, parameters, null, (int?)CommandType.StoredProcedure);

            return firmproduct;
        }

        public static async Task<PagedResults<FirmProduct>> GetFirmProductByAsync(this IdapadDataAccess dataAccess,
             int? firmId, int page, int pageSize, string sort, int? brandId, int? typeId,  string search)
        {
            string sql = "SELECT FP.[Id], F.[Name] FirmName, F.Type FirmType, p.[Name] ProductName, " +
                        "p.[Description], p.[Price], p.[PictureUrl]," +
                        "b.[Name] ProductBrand, t.[Name] ProductType, p.Id ProductId, F.Id FirmId " +
                        "FROM FirmProducts FP " +
                        "INNER JOIN Products p on p.Id = FP.ProductId " +
                        "INNER JOIN Firm F on F.Id = FP.FirmId " +
                        "INNER JOIN ProductBrands b on p.ProductBrandId = b.id " +
                        "INNER JOIN ProductTypes t on p.ProductTypeId = t.id " +
                        "WHERE FP.FirmId = @FirmId " +
                        "AND P.ProductBrandId = isnull(@Brandid, P.ProductBrandId) " +
                        "AND P.ProductTypeId = isnull(@Typeid, P.ProductTypeId) " +
                        "AND p.[Name] like  '%' + isnull(@Search, P.Name) + '%' " +
                        "ORDER BY " +
                        "case when @Sort = 'priceAsc' THEN p.Price END ASC, " +
                        "case when @Sort = 'priceDesc' THEN p.Price END DESC, " +
                        "case when isnull(@Sort, 'name') = 'name' then p.[Name] end ASC, " +
                        "case when @Sort = 'namedesc' then p.[Name] end DESC " +
                        "OFFSET @Offset ROWS " +
                        "FETCH NEXT @PageSize ROWS ONLY";

            string countSql = "SELECT count(*) " +
                                "FROM [dbo].FirmProducts FP " +
                                "INNER JOIN Products p on p.Id = FP.ProductId " +
                                "INNER JOIN Firm F on F.Id = FP.FirmId " +
                                "INNER JOIN ProductBrands b on p.ProductBrandId = b.id " +
                                "INNER JOIN ProductTypes t on p.ProductTypeId = t.id " +
                                "WHERE FP.FirmId = @FirmId " + 
                                "AND P.ProductBrandId = isnull(@Brandid, P.ProductBrandId) " +
                                "AND P.ProductTypeId = isnull(@Typeid, P.ProductTypeId) " +
                                "AND p.[Name] like  '%' + isnull(@Search, P.Name) + '%'";

            var results = new PagedResults<FirmProduct>();

            var TCparameters = new
            {
                FirmId = firmId,
                BrandId = brandId,
                TypeId = typeId,
                Search = search

            };

            var TotalCount = await dataAccess.QueryFirstOrDefaultAsync<int>(countSql
                        , TCparameters);

            var parameters = new
            {
                FirmId = firmId,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize,
                Sort = sort,
                BrandId = brandId,
                TypeId = typeId,
                Search = search
            };

            var firmproducts = await dataAccess.QueryAsync<FirmProduct>(
                sql, parameters);


            results.Items = firmproducts;
            results.TotalCount = TotalCount;

            return results;
        }
    }
}