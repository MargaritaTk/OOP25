using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace КП_ООП
{

    using System.Text.Json;
    using System.Text.Json.Serialization;
    public class UserRegistration
    {
        private readonly List<User> _users = new List<User>();
        private const string FilePath = "users.json";

        public UserRegistration()
        {
            LoadFromFile();
        }

        public void AddUser(User user)
        {
            _users.Add(user ?? throw new ArgumentNullException(nameof(user)));
            SaveToFile();
        }

        public void RemoveUser(User user)
        {
            if (!_users.Contains(user)) throw new ArgumentException("User not found.");
            _users.Remove(user);
            SaveToFile();
        }

        public void ChangeUserRole(User user, Role newRole)
        {
            if (!_users.Contains(user)) throw new ArgumentException("User not found.");
            User updatedUser;
            if (newRole == Role.ProjectManager)
            {
                updatedUser = new ProjectManager(user.Login, user.Password, this);
            }
            else if (newRole == Role.Developer)
            {
                updatedUser = new Developer(user.Login, user.Password);
            }
            else
            {
                throw new ArgumentException("Invalid role.");
            }
            _users.Remove(user);
            _users.Add(updatedUser);
            SaveToFile();
        }

        public bool IsLoginUnique(string login) => !_users.Any(u => u.Login == login);

        public List<User> GetUsers() => _users;

        private void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new UserJsonConverter(this) }
                };
                string json = JsonSerializer.Serialize(_users, options);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні користувачів: {ex.Message}");
            }
        }

        private void LoadFromFile()
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    string json = File.ReadAllText(FilePath);
                    if (string.IsNullOrEmpty(json)) return;

                    var options = new JsonSerializerOptions
                    {
                        Converters = { new UserJsonConverter(this) }
                    };
                    var loadedUsers = JsonSerializer.Deserialize<List<User>>(json, options);
                    if (loadedUsers != null)
                    {
                        _users.Clear();
                        _users.AddRange(loadedUsers);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при завантаженні користувачів: {ex.Message}");
                }
            }
        }
    }

    public class UserJsonConverter : JsonConverter<User>
    {
        private readonly UserRegistration _userRegistry;

        public UserJsonConverter(UserRegistration userRegistry)
        {
            _userRegistry = userRegistry ?? throw new ArgumentNullException(nameof(userRegistry));
        }

        public override User Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("UserRole", out var roleProperty))
            {
                throw new JsonException("Missing UserRole property in JSON.");
            }

            var role = (Role)roleProperty.GetInt32();
            var login = root.TryGetProperty("Login", out var loginProp) ? loginProp.GetString() ?? "" : "";
            var password = root.TryGetProperty("Password", out var passProp) ? passProp.GetString() ?? "" : "";

            return role switch
            {
                Role.ProjectManager => new ProjectManager(login, password, _userRegistry),
                Role.Developer => new Developer(login, password),
                Role.Guest => new Guest(_userRegistry),
                _ => throw new JsonException($"Unknown role: {role}")
            };
        }

        public override void Write(Utf8JsonWriter writer, User value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Login", value.Login);
            writer.WriteString("Password", value.Password);
            writer.WriteNumber("UserRole", (int)value.UserRole);
            writer.WriteString("RoleName", value.UserRole.ToString());
            writer.WriteEndObject();
        }
    }
}
