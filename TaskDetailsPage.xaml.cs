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
    /// Логика взаимодействия для TaskDetailsPage.xaml
    /// </summary>
    public partial class TaskDetailsPage : Window
    {
        private readonly UserRegistration _userRegistry;
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;
        private readonly Project _project;
        private readonly Task _task;

        public TaskDetailsPage(UserRegistration userRegistry, ProjectService projectService, TaskService taskService, CommentService commentService, ExportService exportService, User currentUser, Project project, Task task)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
            _projectService = projectService;
            _taskService = taskService;
            _commentService = commentService;
            _exportService = exportService;
            _currentUser = currentUser;
            _project = project;
            _task = task;
            LoadTaskDetails();
        }

        private void LoadTaskDetails()
        {
            TaskTitleTextBlock.Text = _task.Title;
            TaskDescriptionTextBlock.Text = _task.Description;
            TaskDeadlineTextBlock.Text = $"Deadline: {_task.Deadline:dd.MM.yyyy}";
            TaskStatusTextBlock.Text = $"Status: {_task.Status}";

            CommentsListBox.Items.Clear();
            if (_task.Comments == null || _task.Comments.Count == 0)
            {
                CommentsListBox.Items.Add(new TextBlock { Text = "No Comments.", FontStyle = FontStyles.Italic });
            }
            else
            {
                foreach (var comment in _task.Comments)
                {
                    CommentsListBox.Items.Add(new TextBlock { Text = $"{comment.Author.Login}: {comment.Text} ({comment.CreatedAt:dd.MM.yyyy HH:mm})" });
                }
            }

            if (_currentUser.UserRole != Role.Developer || (_task.AssignedDeveloper != null && _task.AssignedDeveloper.Login != _currentUser.Login))
            {
                ChangeStatusButton.IsEnabled = false;
            }
        }

        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            string commentText = Microsoft.VisualBasic.Interaction.InputBox("Enter a comment:", "Add a comment", "");
            if (!string.IsNullOrEmpty(commentText))
            {
                try
                {
                    _commentService.AddComment(_task, commentText);
                    LoadTaskDetails();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Error adding comment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ChangeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskStatus newStatus = _task.Status == TaskStatus.Open ? TaskStatus.InProgress : TaskStatus.Closed;
                _task.SetStatus(newStatus);
                LoadTaskDetails();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error changing status: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectDetailsPage projectDetailsPage = new ProjectDetailsPage(_userRegistry, _projectService, _taskService, _commentService, _exportService, _currentUser, _project);
            projectDetailsPage.Show();
            this.Close();
        }
    }
}