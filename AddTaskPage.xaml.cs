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
    /// Логика взаимодействия для AddTaskPage.xaml
    /// </summary>
    public partial class AddTaskPage : Window
    {
        private readonly UserRegistration _userRegistry;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;
        private readonly Project _project;

        public AddTaskPage(UserRegistration userRegistry, TaskService taskService, CommentService commentService, ExportService exportService, User currentUser, Project project)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
            _taskService = taskService;
            _commentService = commentService;
            _exportService = exportService;
            _currentUser = currentUser;
            _project = project;
            LoadDevelopers();
            StatusComboBox.SelectedIndex = 0;
        }

        private void LoadDevelopers()
        {
            AssignedDeveloperComboBox.Items.Clear();
            var developers = _userRegistry.GetUsers().Where(u => u.UserRole == Role.Developer).ToList();
            if (developers.Count == 0)
            {
                AssignedDeveloperComboBox.Items.Add(new ComboBoxItem { Content = "No developers", IsEnabled = false });
            }
            else
            {
                foreach (var developer in developers)
                {
                    AssignedDeveloperComboBox.Items.Add(new ComboBoxItem { Content = developer.Login, Tag = developer });
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string title = TaskTitleTextBox.Text;
                string description = TaskDescriptionTextBox.Text;
                string comment = CommentTextBox.Text;
                DateTime? deadline = DeadlineDatePicker.SelectedDate;

                if (string.IsNullOrEmpty(title))
                {
                    MessageBox.Show("The task name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!deadline.HasValue)
                {
                    MessageBox.Show("Choose a deadline!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                TaskStatus status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content switch
                {
                    "Open" => TaskStatus.Open,
                    "InProgress" => TaskStatus.InProgress,
                    "Closed" => TaskStatus.Closed,
                    _ => TaskStatus.Open
                };

                User assignedDeveloper = null;
                if (AssignedDeveloperComboBox.SelectedItem is ComboBoxItem selectedDeveloper && selectedDeveloper.Tag is User developer)
                {
                    assignedDeveloper = developer;
                }

                ProjectService.Instance.AddTaskToProject(_project, title, description, deadline.Value, status, assignedDeveloper);

                if (!string.IsNullOrEmpty(comment))
                {
                    var newTask = _project.Tasks.Last();
                    _commentService.AddComment(newTask, comment);
                    ProjectService.Instance.SaveProjects(_exportService);
                }

                MessageBox.Show("Task successfully created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ProjectDetailsPage projectDetailsPage = new ProjectDetailsPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser, _project);
                projectDetailsPage.Show();
                this.Close();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("closed task"))
            {
                MessageBox.Show("This task is closed and cannot be modified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Operation failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating a task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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