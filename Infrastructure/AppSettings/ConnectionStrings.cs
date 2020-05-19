namespace Infrastructure.AppSettings
{
    public class ConnectionStrings
    {
        public string IdapadDb { get; set; }
        public ConnectionStrings(string idapadDb)
        {
            IdapadDb = idapadDb;
        }
        public ConnectionStrings()
        {
        }
    }

    public class RedisConnectionStrings
    {
        public string Redis { get; set; }
        public RedisConnectionStrings(string redis)
        {
            Redis = redis;
        }
        public RedisConnectionStrings()
        {
            Redis = "localhost";
        }
    }
}