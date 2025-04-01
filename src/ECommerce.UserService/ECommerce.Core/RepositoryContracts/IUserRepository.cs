using ECommerce.Core.Entities;

namespace ECommerce.Core.RepositoryContracts;

public interface IUserRepository
{
    /// <summary>
    /// Add a new user to the database
    /// </summary>
    /// <param name="user">new user</param>
    /// <returns>user with new id</returns>
    Task<ApplicationUser?> AddUser(ApplicationUser user);
    
    /// <summary>
    /// Get a user by email and password
    /// </summary>
    /// <param name="email">email</param>
    /// <param name="password">password</param>
    /// <returns>user</returns>
    Task<ApplicationUser?> GetUserByEmailAndPassword(string email, string password);
    
    /// <summary>
    /// Get a user by UserId
    /// </summary>
    /// <param name="userId">UserId</param>
    /// <returns>user</returns>
    Task<ApplicationUser?> GetUserByUserId(Guid userId);
}