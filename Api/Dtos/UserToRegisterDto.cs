namespace Api.Dtos
{
    public class UserToRegisterDto
    {
       
            public int Id { get; set; }
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string UserName { get; set; }

            public string EmailAddress { get; set; }

            public byte[] PasswordHash { get; set; }

            public byte[] PasswordSalt { get; set; }
    }
}