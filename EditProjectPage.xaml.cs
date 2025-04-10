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
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;
        private readonly Project _project;

        public EditProjectPage(UserRegistration userRegistry, TaskService taskService, CommentService commentService, ExportService exportService, User currentUser, Project project)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
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
                    MessageBox.Show("The project name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ProjectService.Instance.UpdateProject(_project, name, description);
                MessageBox.Show("The project has been successfully updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                ProjectDetailsPage projectDetailsPage = new ProjectDetailsPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser, _project);
                projectDetailsPage.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating a project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectDetailsPage projectDetailsPage = new ProjectDetailsPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser, _project);
            projectDetailsPage.Show();
            this.Close();
        }
    }
}