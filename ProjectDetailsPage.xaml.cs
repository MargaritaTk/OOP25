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
    public partial class ProjectDetailsPage : Window
    {
        private readonly UserRegistration _userRegistry;
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;
        private readonly Project _project;
        private Task _draggedTask;

        public ProjectDetailsPage(UserRegistration userRegistry, ProjectService projectService, TaskService taskService, CommentService commentService, ExportService exportService, User currentUser, Project project)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
            _projectService = projectService;
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
                ToDoListBox.Items.Add(new TextBlock { Text = "Немає завдань.", FontStyle = FontStyles.Italic });
                InProgressListBox.Items.Add(new TextBlock { Text = "Немає завдань.", FontStyle = FontStyles.Italic });
                DoneListBox.Items.Add(new TextBlock { Text = "Немає завдань.", FontStyle = FontStyles.Italic });
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
                    TaskDetailsPage taskDetailsPage = new TaskDetailsPage(_userRegistry, _projectService, _taskService, _commentService, _exportService, _currentUser, _project, task);
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
                ImportProjectButton.IsEnabled = false;
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskPage addTaskPage = new AddTaskPage(_userRegistry, _projectService, _taskService, _commentService, _exportService, _currentUser, _project);
            addTaskPage.Show();
            this.Close();
        }

        private void EditProjectButton_Click(object sender, RoutedEventArgs e)
        {
            EditProjectPage editProjectPage = new EditProjectPage(_userRegistry, _projectService, _taskService, _commentService, _exportService, _currentUser, _project);
            editProjectPage.Show();
            this.Close();
        }

        private void DeleteProjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _projectService.DeleteProject(_project);
                MessageBox.Show("Проєкт успішно видалено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                ProjectListPage projectListPage = new ProjectListPage(_userRegistry, _projectService, _taskService, _commentService, _exportService, _currentUser);
                projectListPage.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при видаленні проєкту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportProjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Project> projectsToExport = new List<Project> { _project };
                string json = _exportService.ExportProjects(projectsToExport);
                File.WriteAllText($"{_project.Name}_export.json", json);
                MessageBox.Show($"Проєкт експортовано у файл {_project.Name}_export.json!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при експорті проєкту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportProjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "JSON файли (*.json)|*.json",
                    Title = "Виберіть файл JSON для імпорту"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    string json = File.ReadAllText(openFileDialog.FileName);
                    var importedProjects = _exportService.ImportProjects(json);
                    foreach (var importedProject in importedProjects)
                    {
                        _projectService.CreateProject(importedProject.Name, importedProject.Description);
                        foreach (var task in importedProject.Tasks)
                        {
                            _projectService.AddTaskToProject(importedProject, task.Title, task.Description, task.Deadline, task.Status, task.AssignedDeveloper);
                        }
                    }
                    MessageBox.Show("Проєкт(и) успішно імпортовано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (importedProjects.Exists(p => p.Name == _project.Name))
                    {
                        LoadProjectDetails();
                    }
                    else
                    {
                        ProjectListPage projectListPage = new ProjectListPage(_userRegistry, _projectService, _taskService, _commentService, _exportService, _currentUser);
                        projectListPage.Show();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при імпорті проєкту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is ListBoxItem selectedItem && selectedItem.Tag is Task task)
            {
                _draggedTask = task;
                DragDrop.DoDragDrop(listBox, task, DragDropEffects.Move);
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
                TaskStatus newStatus = targetListBox.Name switch
                {
                    "ToDoListBox" => TaskStatus.Open,
                    "InProgressListBox" => TaskStatus.InProgress,
                    "DoneListBox" => TaskStatus.Closed,
                    _ => _draggedTask.Status
                };

                _draggedTask.SetStatus(newStatus);
                LoadProjectDetails();
                _draggedTask = null;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectListPage projectListPage = new ProjectListPage(_userRegistry, _projectService, _taskService, _commentService, _exportService, _currentUser);
            projectListPage.Show();
            this.Close();
        }
    }
}