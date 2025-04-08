using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class ProjectService : IProject
    {
        private static ProjectService _instance;
        private readonly List<Project> _projects = new List<Project>();
        private readonly ExportService _exportService;
        private const string FilePath = "projects.json";

        private ProjectService()
        {
            _exportService = new ExportService();
            LoadFromFile();
        }

        public static ProjectService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProjectService();
                }
                return _instance;
            }
        }

        public void CreateProject(string name, string description)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Project name cannot be empty.");
            if (_projects.Exists(p => p.Name == name)) throw new ArgumentException("Project with this name already exists.");
            _projects.Add(new Project(name, description));
            SaveToFile();
        }

        public void UpdateProject(Project project, string newName, string newDescription)
        {
            if (!_projects.Contains(project)) throw new ArgumentException("Project not found.");
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("New name cannot be empty.");
            if (_projects.Exists(p => p.Name == newName && p != project)) throw new ArgumentException("Project with this name already exists.");
            project.Update(newName, newDescription);
            SaveToFile();
        }

        public void DeleteProject(Project project)
        {
            if (!_projects.Contains(project)) throw new ArgumentException("Project not found.");
            if (project.HasActiveTasks())
                throw new InvalidOperationException("Cannot delete project with active tasks.");
            _projects.Remove(project);
            SaveToFile();
        }

        public void AddTaskToProject(Project project, string title, string description, DateTime deadline, TaskStatus status, User assignedDeveloper)
        {
            if (!_projects.Contains(project)) throw new ArgumentException("Project not found.");
            var task = new Task(title, description, deadline);
            task.SetStatus(status);
            if (assignedDeveloper != null)
            {
                task.AssignDeveloper(assignedDeveloper as Developer);
            }
            project.Tasks.Add(task);
            SaveToFile();
        }

        public List<Project> GetProjects() => _projects;

        public List<Project> GetProjects(User user = null)
        {
            if (user == null || user.UserRole == Role.ProjectManager)
            {
                return _projects;
            }
            else if (user.UserRole == Role.Developer)
            {
                return _projects
                    .Where(p => p.Tasks.Any())
                    .ToList();
            }
            return new List<Project>();
        }

        public void SaveToFile()
        {
            try
            {
                string json = _exportService.ExportProjects(_projects);
                File.WriteAllText(FilePath, json);
                Console.WriteLine($"Projects saved to file: {FilePath}, number of projects: {_projects.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving projects: {ex.Message}");
            }
        }

        private void LoadFromFile()
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    string json = File.ReadAllText(FilePath);
                    if (string.IsNullOrEmpty(json)) return;

                    var loadedProjects = _exportService.ImportProjects(json);
                    _projects.Clear();
                    _projects.AddRange(loadedProjects);
                    Console.WriteLine($"Downoloaded {loadedProjects.Count} project(s) from file: {FilePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro downoloading projects: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"File {FilePath} not found.");
            }
        }
    }
}
