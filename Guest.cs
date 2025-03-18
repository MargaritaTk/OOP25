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

        public Guest(UserRegistration userRegistry)
      : base("", "", Role.Guest)
        {
            _userRegistry = userRegistry ?? throw new ArgumentNullException(nameof(userRegistry));
        }

        //Подія для повідомлення про реєстрацію 
        public event Action<User> UserRegistered;

        // Перевизначений метод для реєстарції
        public User UpdateProfileForRegistration(string login, string password, Role targetRole)
        {
            if (string.IsNullOrEmpty(login) || login.Length < 3)
                throw new ArgumentException("Login must be at least 3 characters.");
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters.");

            if (!_userRegistry.IsLoginUnique(login))
                throw new InvalidOperationException("Login is already in use.");

            // Створення нового користувача залежно від ролі
            User registeredUser;
            switch (targetRole)
            {
                case Role.ProjectManager:
                    registeredUser = new ProjectManager(login, password, _userRegistry);
                    break;
                case Role.Developer:
                    registeredUser = new Developer(login, password, _userRegistry);
                    break;
                default:
                    throw new ArgumentException("Invalid target role for registration.");
            }

            _userRegistry.AddUser(registeredUser);
            UserRegistered?.Invoke(registeredUser); 
            return registeredUser; 
        }
    }
}
        