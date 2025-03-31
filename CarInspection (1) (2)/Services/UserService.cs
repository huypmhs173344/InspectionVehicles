using BusinessObject.Models;
using Repositories;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService()
        {
            _unitOfWork = new UnitOfWork(new CarInspectionDbContext());
        }

        public void CreateUser(User user)
        {
            var maxId = _unitOfWork.UserRepository.GetAll().Max(u => u.UserId);
            user.UserId = maxId + 1;
            user.Password = PasswordHasher.HashPassword(user.Password);
            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.Save();
        }

        public void DeleteUser(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            if (user != null)
            {
                _unitOfWork.UserRepository.Delete(user);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _unitOfWork.UserRepository.GetAll();
        }

        public User GetUserById(int id)
        {
            return _unitOfWork.UserRepository.GetById(id);
        }

        public User GetUserByEmail(string email)
        {
            return _unitOfWork
                .UserRepository.GetAll()
                .FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        public bool ValidateUser(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
                return false;

            return PasswordHasher.VerifyPassword(password, user.Password);
        }

        public bool IsEmailExists(string email)
        {
            return _unitOfWork
                .UserRepository.GetAll()
                .Any(u => u.Email.ToLower() == email.ToLower());
        }

        public void UpdateUser(User user)
        {
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();
        }
        public void UpdateUserRole(int userId, string newRole)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            if (user != null)
            {
                user.Role = newRole;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
            }
        }
        public IEnumerable<User> SearchUsers(string keyword)
        {
            return _unitOfWork
                .UserRepository.GetAll()
                .Where(u => u.FullName.ToLower().Contains(keyword.ToLower()) ||
                            u.Email.ToLower().Contains(keyword.ToLower()));
        }
    }
}
