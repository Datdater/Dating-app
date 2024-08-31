using HumanShop.Data;
using HumanShop.Entities;
using HumanShop.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HumanShop.Services.Implementation
{
    public class UserRespository(ApplicationDBContext context) : IUserRespository
    {
        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
                
            return await context.appUsers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser?> GetUserByNameAsync(string name)
        {
            return await context.appUsers.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.appUsers
                .Include(x => x.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
