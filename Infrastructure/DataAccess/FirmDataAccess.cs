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
             Address address)
        {
                return await dataAccess.ExecuteScalarAsync<int>(
                    "Insert Address (Type, Row1, Row2, City, State, Zip1)  output inserted.Id values" +
                    "(@Type, @Row1, @Row2, @City, @State, @Zip1)", address);
            
        }

        public static async Task<int> LinkFirmToUserAsync(this IdapadDataAccess dataAccess,
             FirmUser firmUser)
        {
            return await dataAccess.ExecuteScalarAsync<int>(
                "Insert FirmUsers (FirmId, UserId)  output inserted.Id values" +
                "(@FirmId, @UserId)", firmUser);

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