public enum Role { Guest, Developer, ProjectManager }

public abstract class User
{
    public string Login { get; set; }
    public string Password { get; set; }
    public Role UserRole { get; }

    public User(string login, string password, Role role)
    {
        Login = login;
        Password = password;
        UserRole = role;
    }

    public virtual void UpdateProfile(string login, string password)
    {
        throw new NotImplementedException(); // Заглушка
    }

    public event Action<User> ProfileUpdated;
}