namespace MotoShop.Services.Redis
{
    public struct RedisConnectionResult
    {
        public bool Connected { get; set; }
        public string Description { get; set; }

        public static RedisConnectionResult ConnectionSucceeded(string description)
        {
            RedisConnectionResult result = default;
            result.Connected = true;
            result.Description = description;

            return result;
        }
        public static RedisConnectionResult ConnectionFailed(string description)
        {
            RedisConnectionResult result = default;
            result.Connected = false;
            result.Description = description;

            return result;
        }
    }
}
