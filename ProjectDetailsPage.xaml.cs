using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ProjectDetailsPage.xaml
    /// </summary>
    /// 
    public partial class ProjectDetailsPage : Window
    {
        private readonly UserRegistration _userRegistry;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;
        private readonly Project _project;
        private Task _draggedTask;

        public ProjectDetailsPage(UserRegistration userRegistry, TaskService taskService, CommentService commentService, ExportService exportService, User currentUser, Project project)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
            _taskService = taskService;
            _commentService = commentService;
            _exportService = exportService;
            _currentUser = currentUser;
            _project = project;
            LoadProjectDetails();
        }

        private void LoadProjectDetails()
        {
            ProjectNameTextBlock.Text = _project.Name;
            ProjectDescriptionTextBlock.Text = _project.Description;

            ToDoListBox.Items.Clear();
            InProgressListBox.Items.Clear();
            DoneListBox.Items.Clear();

            if (_project.Tasks == null || _project.Tasks.Count == 0)
            {
                ToDoListBox.Items.Add(new TextBlock { Text = "No tasks.", FontStyle = FontStyles.Italic });
                InProgressListBox.Items.Add(new TextBlock { Text = "No tasks.", FontStyle = FontStyles.Italic });
                DoneListBox.Items.Add(new TextBlock { Text = "No tasks.", FontStyle = FontStyles.Italic });
                return;
            }

            foreach (var task in _project.Tasks)
            {
                var taskItem = new ListBoxItem
                {
                    Content = task.Title,
                    Tag = task
                };
                taskItem.MouseDoubleClick += (s, e) =>
                {
                    TaskDetailsPage taskDetailsPage = new TaskDetailsPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser, _project, task);
                    taskDetailsPage.Show();
                    this.Close();
                };

                if (task.Status == TaskStatus.Open)
                    ToDoListBox.Items.Add(taskItem);
                else if (task.Status == TaskStatus.InProgress)
                    InProgressListBox.Items.Add(taskItem);
                else if (task.Status == TaskStatus.Closed)
                    DoneListBox.Items.Add(taskItem);
            }

            if (_currentUser.UserRole != Role.ProjectManager)
            {
                AddTaskButton.IsEnabled = false;
                EditProjectButton.IsEnabled = false;
                DeleteProjectButton.IsEnabled = false;
                ExportProjectButton.IsEnabled = false;
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskPage addTaskPage = new AddTaskPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser, _project);
            addTaskPage.Show();
            this.Close();
        }

        private void EditProjectButton_Click(object sender, RoutedEventArgs e)
        {
            EditProjectPage editProjectPage = new EditProjectPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser, _project);
            editProjectPage.Show();
            this.Close();
        }

        private void DeleteProjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProjectService.Instance.DeleteProject(_project);
                MessageBox.Show("Project successfully deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ProjectListPage projectListPage = new ProjectListPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser);
                projectListPage.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting a project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportProjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Project> projectsToExport = new List<Project> { _project };
                string filePath = $"{_project.Name}_export.json";
                _exportService.ExportProjectsToFile(projectsToExport, filePath);
                MessageBox.Show($"Project exported to file {filePath}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Failed to export project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting a project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is ListBoxItem selectedItem && selectedItem.Tag is Task task)
            {
                if (_currentUser.UserRole == Role.ProjectManager ||
                    (_currentUser.UserRole == Role.Developer && task.AssignedDeveloper != null && task.AssignedDeveloper.Login == _currentUser.Login))
                {
                    _draggedTask = task;
                    DragDrop.DoDragDrop(listBox, task, DragDropEffects.Move);
                }
            }
        }

        private void ListBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (sender is ListBox targetListBox && _draggedTask != null)
            {
                try
                {
                    TaskStatus newStatus = targetListBox.Name switch
                    {
                        "ToDoListBox" => TaskStatus.Open,
                        "InProgressListBox" => TaskStatus.InProgress,
                        "DoneListBox" => TaskStatus.Closed,
                        _ => _draggedTask.Status
                    };

                    _draggedTask.SetStatus(newStatus);
                    ProjectService.Instance.SaveProjects(_exportService);
                    LoadProjectDetails();
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("closed task"))
                {
                    MessageBox.Show("This task is closed and its status cannot be changed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error moving task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    _draggedTask = null;
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectListPage projectListPage = new ProjectListPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser);
            projectListPage.Show();
            this.Close();
        }
    }
}
