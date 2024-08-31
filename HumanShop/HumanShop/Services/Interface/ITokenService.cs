using HumanShop.Entities;

namespace HumanShop.Services.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
