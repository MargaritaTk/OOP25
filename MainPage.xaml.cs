﻿using System;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        private readonly UserRegistration _userRegistry;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;
        private readonly User _currentUser;

        public MainPage(UserRegistration userRegistry, User currentUser)
        {
            InitializeComponent();
            _userRegistry = userRegistry;
            _exportService = new ExportService();
            _taskService = new TaskService();
            _commentService = new CommentService(currentUser);
            _currentUser = currentUser;


            if (_currentUser is ProjectManager projectManager)
            {
                projectManager.InitializeServices(ProjectService.Instance, _taskService, _commentService, _exportService);
            }

            LoadRecentProjects();
        }

        private void LoadRecentProjects()
        {
            var recentProjects = ProjectService.Instance.GetProjects(_currentUser).Take(5).ToList();
            RecentProjectsListBox.Items.Clear();
            if (recentProjects.Count == 0)
            {
                RecentProjectsListBox.Items.Add(new TextBlock { Text = "No recent projects.", FontStyle = FontStyles.Italic });
            }
            else
            {
                foreach (var project in recentProjects)
                {
                    RecentProjectsListBox.Items.Add(new TextBlock { Text = $"{project.Name} - Created" });
                }
            }
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            CreateProjectPage createProjectPage = new CreateProjectPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser);
            createProjectPage.Show();
            this.Close();
        }

        private void ProjectListButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectListPage projectListPage = new ProjectListPage(_userRegistry, _taskService, _commentService, _exportService, _currentUser);
            projectListPage.Show();
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}