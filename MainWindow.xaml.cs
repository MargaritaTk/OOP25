using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectServ;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly UserRegistration _userRegistry;

    public MainWindow()
    {
        InitializeComponent();
        _userRegistry = new UserRegistration();
    }

    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            bool isProjectManager = ProjectManagerCheckBox.IsChecked == true;
            bool isDeveloper = DeveloperCheckBox.IsChecked == true;

            if (isProjectManager && isDeveloper)
            {
                MessageBox.Show("Оберіть лише одну роль!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!isProjectManager && !isDeveloper)
            {
                MessageBox.Show("Оберіть роль!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Role targetRole = isProjectManager ? Role.ProjectManager : Role.Developer;
            Guest guest = new Guest(_userRegistry);
            User newUser = guest.Register(login, password, targetRole);

            MessageBox.Show($"Користувач {newUser.Login} успішно зареєстрований як {newUser.UserRole}!",
                "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

            MainPage mainPage = new MainPage(_userRegistry, newUser);
            mainPage.Show();
            Window.GetWindow(this).Close();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Виникла неочікувана помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LoginLink_Click(object sender, RoutedEventArgs e)
    {
        LoginWindow loginWindow = new LoginWindow(_userRegistry);
        loginWindow.Show();
    }
}
