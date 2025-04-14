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
                MessageBox.Show("Choose only one role!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!isProjectManager && !isDeveloper)
            {
                MessageBox.Show("Choose a role!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Role targetRole = isProjectManager ? Role.ProjectManager : Role.Developer;
            Guest guest = new Guest(_userRegistry);

            guest.UserRegistered += OnUserRegistered;

            User newUser = guest.Register(login, password, targetRole);

            MainPage mainPage = new MainPage(_userRegistry, newUser);
            mainPage.Show();
            Window.GetWindow(this).Close();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show($"Operation failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void OnUserRegistered(User newUser)
    {
        Dispatcher.Invoke(() =>
        {
            MessageBox.Show($"User {newUser.Login} has been registered with role {newUser.UserRole}!",
                "Registration Success", MessageBoxButton.OK, MessageBoxImage.Information);
        });
    }

    private void LoginLink_Click(object sender, RoutedEventArgs e)
    {
        LoginWindow loginWindow = new LoginWindow(_userRegistry);
        loginWindow.Show();
    }
}