using BusinessObject.Models;
using System.Collections.Generic;

namespace Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByEmail(string email);
        bool ValidateUser(string email, string password);
        bool IsEmailExists(string email);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        void UpdateUserRole(int userId, string newRole);
        IEnumerable<User> SearchUsers(string keyword);
    }
}
