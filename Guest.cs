using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class Guest : User
    {
        private readonly UserRegistration _userRegistry;

        public event Action<User> UserRegistered;

        public Guest(UserRegistration userRegistry)
            : base("", "", Role.Guest)
        {
            _userRegistry = userRegistry ?? throw new ArgumentNullException(nameof(userRegistry));
        }

        public User Register(string login, string password, Role targetRole)
        {
            if (string.IsNullOrEmpty(login) || login.Length < 3)
                throw new ArgumentException("Login must be at least 3 characters.");
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters.");
            if (!_userRegistry.IsLoginUnique(login))
                throw new InvalidOperationException("Login is already in use.");
            if (targetRole == Role.Guest) throw new ArgumentException("Cannot register as Guest.");

            User user = targetRole switch
            {
                Role.ProjectManager => new ProjectManager(login, password, _userRegistry),
                Role.Developer => new Developer(login, password),
                _ => throw new ArgumentException("Invalid role for registration.")
            };

            _userRegistry.AddUser(user);
            UserRegistered?.Invoke(user);
            return user;
        }
    }
}
