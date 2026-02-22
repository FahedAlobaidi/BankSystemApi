using BankSystemApi.Entities;

namespace BankSystemApi.TokenService
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
