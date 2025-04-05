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
    /// Логика взаимодействия для EditProjectPage.xaml
    /// </summary>
    public partial class EditProjectPage : Window
    {
        private readonly UserRegistration _userRegistry;
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;
        private readonly Project _project;

        public EditProjectPage(UserRegistration userRegistry, ProjectService projectService, TaskService taskService, CommentService commentService, ExportService exportService, User currentUser, Project project)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
            _projectService = projectService;
            _taskService = taskService;
            _commentService = commentService;
            _exportService = exportService;
            _currentUser = currentUser;
            _project = project;

            ProjectNameTextBox.Text = _project.Name;
            ProjectDescriptionTextBox.Text = _project.Description;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
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

                _projectService.UpdateProject(_project, name, description);
                MessageBox.Show("Проєкт успішно оновлено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                ProjectDetailsPage projectDetailsPage = new ProjectDetailsPage(_userRegistry, _projectService, _taskService, _commentService, _exportService, _currentUser, _project);
                projectDetailsPage.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при оновленні проєкту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectDetailsPage projectDetailsPage = new ProjectDetailsPage(_userRegistry, _projectService, _taskService, _commentService, _exportService, _currentUser, _project);
            projectDetailsPage.Show();
            this.Close();
        }
    }
}
