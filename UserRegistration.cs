using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class UserRegistration
    {
        private readonly List<User> _users = new List<User>();

        public void AddUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _users.Add(user);
        }

        public bool IsLoginUnique(string login)
        {
            return !_users.Any(u => u.Login == login);
        }
    }
}
