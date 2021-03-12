namespace MotoShop.Services.Services
{
    public interface IRefreshTokenGenerator<T>
    {
        T Generate(string userID);
    }
}
