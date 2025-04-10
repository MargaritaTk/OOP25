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
    /// Логика взаимодействия для ProjectListPage.xaml
    /// </summary>
    public partial class ProjectListPage : Window
    {
        private readonly UserRegistration _userRegistry;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;

        public ProjectListPage(UserRegistration userRegistry, TaskService taskService, CommentService commentService, ExportService exportService, User currentUser)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
            _taskService = taskService;
            _commentService = commentService;
            _exportService = exportService;
            _currentUser = currentUser;
            LoadProjects();
        }

        private void LoadProjects()
        {
            ProjectsListBox.Items.Clear();
            var projects = ProjectService.Instance.GetProjects(_currentUser);
            if (projects == null || projects.Count == 0)
            {
                ProjectsListBox.Items.Add(new TextBlock { Text = "No projects to display.", FontStyle = FontStyles.Italic });
                return;
            }

            foreach (var project in projects)
            {
                ProjectsListBox.Items.Add(new TextBlock { Text = project.Name, Tag = project });
            }
        }

        private void ProjectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectsListBox.SelectedItem is TextBlock selectedItem && selectedItem.Tag is Project selectedProject)
            {
                ProjectDetailsPage projectDetailsPage = new ProjectDetailsPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser, selectedProject);
                projectDetailsPage.Show();
                this.Close();
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