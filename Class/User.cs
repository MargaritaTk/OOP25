using System.Data;

namespace ProjectServ
{
    public abstract class User
    {
        public string Login { get; protected set; }
        public string Password { get; protected set; }
        public Role UserRole { get; }

        protected User(string login, string password, Role role)
        {
            if (role != Role.Guest && (string.IsNullOrEmpty(login) || login.Length < 3))
                throw new ArgumentException("Login must be at least 3 characters.");
            if (role != Role.Guest && (string.IsNullOrEmpty(password) || password.Length < 8))
                throw new ArgumentException("Password must be at least 8 characters.");
            Login = login;
            Password = password;
            UserRole = role;
        }

        public virtual void UpdateProfile(string login, string password)
        {
            if (UserRole == Role.Guest)
                throw new InvalidOperationException("Guest cannot update profile.");
            if (string.IsNullOrEmpty(login) || login.Length < 3)
                throw new ArgumentException("Login must be at least 3 characters.");
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters.");
            Login = login;
            Password = password;
        }
    }
    public enum Role
    {
        ProjectManager,
        Developer,
        Guest
    }
}

    //public abstract class User
    //{
    //    public string Login { get; set; }
    //    public string Password { get; set; }
    //    public Role UserRole { get; }

    //    public User(string login, string password, Role role)
    //    {
    //        if (role != Role.Guest)
    //        {
    //            if (string.IsNullOrEmpty(login) || login.Length < 3)
    //                throw new ArgumentException("Login must be at least 3 characters.");
    //            if (string.IsNullOrEmpty(password) || password.Length < 8)
    //                throw new ArgumentException("Password must be at least 8 characters.");
    //        }
    //        Login = login;
    //        Password = password;
    //        UserRole = role;
    //    }

    //    public virtual void UpdateProfile(string login, string password)
    //    {
    //        if (UserRole == Role.Guest)
    //            throw new InvalidOperationException("Guest can only register using a registration method.");

    //        if (string.IsNullOrEmpty(login) || login.Length < 3)
    //            throw new ArgumentException("Login must be at least 3 characters.");
    //        if (string.IsNullOrEmpty(password) || password.Length < 8)
    //            throw new ArgumentException("Password must be at least 8 characters.");
    //        Login = login;
    //        Password = password;
    //        ProfileUpdated?.Invoke(this);
    //    }

    //    public event Action<User> ProfileUpdated;
    //}

   