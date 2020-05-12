using System.Threading.Tasks;
using Infrastructure.Models;



namespace Infrastructure.DataAccess
{
    public static class UserDataAccess
    {

        public static Task<User> GetUserAsync(this IdapadDataAccess dataAccess, int id)
        {
            return dataAccess.QueryFirstOrDefaultAsync<User>
                ("select a.Id, a.FirstName, a.LastName, a.UserName, a.EmailAddress, " +
                "f.Name FirmName, f.Type FirmType from Appuser a " +
                "left join FirmUsers fu On a.Id = fu.UserId  " +
                "inner join Firm f On f.Id = fu.firmId  " +
                "where a.Id = @id", new { id });
        }

        public static Task<User> GetUserByUserNameAsync(this IdapadDataAccess dataAccess, string userName)
        {
            return dataAccess.QueryFirstOrDefaultAsync<User>
                ("select a.Id, a.FirstName, a.LastName, a.UserName, a.EmailAddress " +
                "from Appuser a " +
                "where a.UserName = @UserName", new { userName });
        }

        public static Task<User> GetFirmUserByUserNameAsync(this IdapadDataAccess dataAccess, string userName)
        {
            return dataAccess.QueryFirstOrDefaultAsync<User>
                ("select a.Id, a.FirstName, a.LastName, a.UserName, a.EmailAddress, " +
                "f.Name FirmName, f.Type FirmType from Appuser a " +
                "left join FirmUsers fu On a.Id = fu.UserId  " +
                "left join Firm f On f.Id = fu.firmId  " +
                "where a.UserName = @UserName", new { userName });
        }

        public static async Task<bool> UserExists(this IdapadDataAccess dataAccess, string userName)
        {
            User user = await dataAccess.QueryFirstOrDefaultAsync<User>
                    ("select * from Appuser where username = @username", new { userName });
            return (user != null) ? true : false;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static async Task<User> Register(this IdapadDataAccess dataAccess, UserRegister userRegister)
        {

            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(userRegister.Password, out passwordHash, out passwordSalt);

            var userToRegister = new  UserToRegister
            {
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                UserName = userRegister.UserName.ToLower(),
                EmailAddress = userRegister.EmailAddress,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            var id = await dataAccess.ExecuteScalarAsync<int>(
                @"INSERT Appuser(FirstName, LastName, UserName, EmailAddress, PasswordHash, PasswordSalt)" +
                " output inserted.Id VALUES(@FirstName, @LastName, @UserName, @EmailAddress, @PasswordHash, @PasswordSalt)",
                userToRegister);

            var user = new User();
            user.Id = id;
            user.FirstName = userToRegister.FirstName;
            user.LastName = userToRegister.LastName;
            user.UserName = userToRegister.UserName;
            user.EmailAddress = userToRegister.EmailAddress;
            return user;
        }

        public static async Task<User> Login(this IdapadDataAccess dataAccess, UserRegister userRegister)
        {
            var userName = userRegister.UserName.ToLower();

            UserToRegister userToRegister = await dataAccess.QueryFirstOrDefaultAsync<UserToRegister>
              ("select a.*,fu.FirmId, f.Name FirmName from Appuser a " +
                "left join FirmUsers fu On a.Id = fu.UserId  " +
                "left join Firm f On f.Id = fu.firmId  " +
                "where a.UserName = @UserName", new { userName });

            if (userToRegister == null)
                return null;
            if (!VerifyPasswordHash(userRegister.Password, userToRegister.PasswordHash, userToRegister.PasswordSalt))
                return null;

            var user = new User();
            user.Id = userToRegister.Id;
            user.FirstName = userToRegister.FirstName;
            user.LastName = userToRegister.LastName;
            user.UserName = userToRegister.UserName;
            user.EmailAddress = userToRegister.EmailAddress;
            user.FirmName = userToRegister.FirmName;
            user.FirmId = userToRegister.FirmId;
            return user;

        }


        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

            }
            return true;
        }


    }
}