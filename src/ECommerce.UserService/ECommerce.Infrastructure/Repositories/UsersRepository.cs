using ECommerce.Core.DTO;
using ECommerce.Core.Entities;
using ECommerce.Core.RepositoryContracts;

namespace ECommerce.Infrastructure.Repositories;

internal class UsersRepository : IUserRepository
{
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();

        return user;
    }
    
    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string email, string password)
    {
        return new ApplicationUser
        {
            UserId = Guid.NewGuid(),
            Email = email,
            Password = password,
            PersonName = "John Doe",
            Gender = GenderOptions.Male.ToString()
        };
    }
}