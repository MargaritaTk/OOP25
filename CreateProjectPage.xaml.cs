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
    /// Логика взаимодействия для CreateProjectPage.xaml
    /// </summary>
    /// 
    public partial class CreateProjectPage : Window
    {
        private readonly UserRegistration _userRegistry;
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;

        public CreateProjectPage(UserRegistration userRegistry, ProjectService projectService, TaskService taskService, CommentService commentService, ExportService exportService, User currentUser)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
            _projectService = projectService;
            _taskService = taskService;
            _commentService = commentService;
            _exportService = exportService;
            _currentUser = currentUser;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = ProjectNameTextBox.Text;
                string description = ProjectDescriptionTextBox.Text;

                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Назва проєкту не може бути порожньою!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_currentUser is ProjectManager projectManager)
                {
                    projectManager.CreateProject(name, description);
                    MessageBox.Show("Проєкт успішно створено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    ProjectListPage projectListPage = new ProjectListPage(_userRegistry, _projectService, _taskService, _commentService, _exportService, _currentUser);
                    projectListPage.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Тільки Project Manager може створювати проєкти.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при створенні проєкту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage mainPage = new MainPage(_userRegistry, _currentUser);
            mainPage.Show();
            this.Close();
        }
    }
}