using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServ
{
    public class ProjectService : IProject
    {
        private static ProjectService _instance;
        private readonly List<Project> _projects = new List<Project>();

        private ProjectService()
        {
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
        }

        public void UpdateProject(Project project, string newName, string newDescription)
        {
            if (!_projects.Contains(project)) throw new ArgumentException("Project not found.");
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("New name cannot be empty.");
            if (_projects.Exists(p => p.Name == newName && p != project)) throw new ArgumentException("Project with this name already exists.");
            project.Update(newName, newDescription);
        }

        public void DeleteProject(Project project)
        {
            if (!_projects.Contains(project)) throw new ArgumentException("Project not found.");
            if (project.HasActiveTasks())
                throw new InvalidOperationException("Cannot delete project with active tasks.");
            _projects.Remove(project);
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

        public void SaveProjects(IExport exportService)
        {
            if (exportService == null) throw new ArgumentNullException(nameof(exportService));
            exportService.SaveToFile(_projects);
        }
    }
}