namespace MotoShop.WebAPI.Configurations
{
    public class RedisOptions
    {
        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }
        public string Host { get; set;}
        public int Port { get; set; }
    }
}
