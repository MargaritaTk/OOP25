using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectServ
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly UserRegistration _userRegistry;

        public LoginWindow(UserRegistration userRegistry)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Enter your login and password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = _userRegistry.GetUsers().FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user == null)
            {
                MessageBox.Show("Incorrect login or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show($"Login is performed as {user.UserRole}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            MainPage mainPage = new MainPage(_userRegistry, user);
            mainPage.Show();
            this.Close();
        }
    }
}