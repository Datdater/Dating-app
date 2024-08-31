using HumanShop.Entities;

namespace HumanShop.Services.Interface
{
    public interface IUserRespository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<AppUser?> GetUserByNameAsync(string name);
    }
}
