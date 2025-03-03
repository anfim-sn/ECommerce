using ECommerce.Core.Entities;

namespace ECommerce.Core.RepositoryContracts;

public interface IUserRepository
{
    Task<ApplicationUser?> AddUser(ApplicationUser user);
    Task<ApplicationUser?> GetUserByEmailAndPassword(string email, string password);
}