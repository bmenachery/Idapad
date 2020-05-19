using System.Threading.Tasks;
using Infrastructure.Models;

namespace Infrastructure.DataAccess
{
    public static class FirmDataAccess
    {
        public static async Task<int> CreateFirmAsync(this IdapadDataAccess dataAccess,
             Firm firm)
        {
            var firmId = await DoestheFirmExists(dataAccess, firm);

            if (firmId < 1)
            {
                return await dataAccess.ExecuteScalarAsync<int>(
                    "Insert Firm (Name, Type, AddressId)  output inserted.Id values(@Name, @Type, @AddressId)", firm);
            }
            return firmId;


        }

        public static async Task<int> CreateFirmAddressAsync(this IdapadDataAccess dataAccess,
             FirmAddress firmaddress)
        {
            var firmAddressId = await DoestheAddressExists(dataAccess, firmaddress);

            if (firmAddressId < 1)
            {
                return await dataAccess.ExecuteScalarAsync<int>(
                    "Insert FirmAddress (TypeId, StreetAddress, AptAddress, City, State, ZipCode)  output inserted.Id values" +
                    "(1, @StreetAddress, @AptAddress, @City, @State, @ZipCode)", firmaddress);
            }

            return firmAddressId;        
            
        }

        public static async Task<int> LinkFirmToUserAsync(this IdapadDataAccess dataAccess,
             FirmUser firmUser)
        {
            return await dataAccess.ExecuteScalarAsync<int>(
                "Insert FirmUsers (FirmId, UserId)  output inserted.Id values" +
                "(@FirmId, @UserId)", firmUser);

        }

        private static async Task<int> DoestheAddressExists(this IdapadDataAccess dataAccess,
            FirmAddress firmaddress)
        {

            int firmAddressId = await dataAccess.QueryFirstOrDefaultAsync<int>(
            "select Id from FirmAddress where " +
            "StreetAddress = @StreetAddress " +
            "AND isnull(AptAddress, 'AptAddress') = ISNULL(@AptAddress, isnull(AptAddress, 'AptAddress')) " + 
            "AND City = @City AND State =@State AND ZipCode= @ZipCode", firmaddress);

            return firmAddressId;

        }


        private static async Task<int> DoestheFirmExists(this IdapadDataAccess dataAccess,
            Firm firm)
        {

            int firmId = await dataAccess.QueryFirstOrDefaultAsync<int>(
            "select * from Firm where Name = @Name", firm);

            return firmId;

        }

    }
}