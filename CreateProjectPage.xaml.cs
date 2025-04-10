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
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;

        public CreateProjectPage(UserRegistration userRegistry, TaskService taskService, CommentService commentService, ExportService exportService, User currentUser)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
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
                    MessageBox.Show("The project name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_currentUser is ProjectManager projectManager)
                {
                    projectManager.CreateProject(name, description);
                    MessageBox.Show("The project has been successfully created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ProjectListPage projectListPage = new ProjectListPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser);
                    projectListPage.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Only Project Manager can create projects.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating a project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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