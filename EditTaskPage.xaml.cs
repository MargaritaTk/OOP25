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
    /// Логика взаимодействия для EditTaskPage.xaml
    /// </summary>
    public partial class EditTaskPage : Window
    {
        private readonly UserRegistration _userRegistry;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;
        private readonly Project _project;
        private readonly Task _task;

        public EditTaskPage(UserRegistration userRegistry, TaskService taskService, CommentService commentService, ExportService exportService, User currentUser, Project project, Task task)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
            _taskService = taskService;
            _commentService = commentService;
            _exportService = exportService;
            _currentUser = currentUser;
            _project = project;
            _task = task ?? throw new ArgumentNullException(nameof(task));
            LoadTaskDetails();
            LoadDevelopers();
            RestrictFieldsForDeveloper();
        }

        private void LoadTaskDetails()
        {
            TaskTitleTextBox.Text = _task.Title;
            TaskDescriptionTextBox.Text = _task.Description;
            DeadlineDatePicker.SelectedDate = _task.Deadline;
            StatusComboBox.SelectedItem = StatusComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == _task.Status.ToString());
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
                    var item = new ComboBoxItem { Content = developer.Login, Tag = developer };
                    AssignedDeveloperComboBox.Items.Add(item);
                    if (_task.AssignedDeveloper != null && developer.Login == _task.AssignedDeveloper.Login)
                    {
                        AssignedDeveloperComboBox.SelectedItem = item;
                    }
                }
            }
        }

        private void RestrictFieldsForDeveloper()
        {
            if (_currentUser.UserRole != Role.ProjectManager)
            {
                TaskTitleTextBox.IsEnabled = false;
                TaskDescriptionTextBox.IsEnabled = false;
                DeadlineDatePicker.IsEnabled = false;
                AssignedDeveloperComboBox.IsEnabled = false;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentUser.UserRole == Role.ProjectManager)
                {
                    string title = TaskTitleTextBox.Text;
                    string description = TaskDescriptionTextBox.Text;
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

                    User assignedDeveloper = null;
                    if (AssignedDeveloperComboBox.SelectedItem is ComboBoxItem selectedDeveloper && selectedDeveloper.Tag is User developer)
                    {
                        assignedDeveloper = developer;
                    }

                    _taskService.UpdateTask(_task, title, description);
                    _taskService.UpdateDeadline(_task, deadline.Value);
                    _taskService.AssignDeveloper(_task, assignedDeveloper as Developer);
                }

                TaskStatus status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content switch
                {
                    "Open" => TaskStatus.Open,
                    "InProgress" => TaskStatus.InProgress,
                    "Closed" => TaskStatus.Closed,
                    _ => TaskStatus.Open
                };
                _taskService.UpdateStatus(_task, status);

                ProjectService.Instance.SaveProjects(_exportService);

                MessageBox.Show("Task successfully updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ProjectDetailsPage projectDetailsPage = new ProjectDetailsPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser, _project);
                projectDetailsPage.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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