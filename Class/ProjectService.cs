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
        private readonly List<Project> _projects = new List<Project>();
        private readonly ExportService _exportService;
        private const string FilePath = "projects.json";

        public ProjectService()
        {
            _exportService = new ExportService();
            LoadFromFile();
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

        private void SaveToFile()
        {
            try
            {
                string json = _exportService.ExportProjects(_projects);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні проєктів: {ex.Message}");
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при завантаженні проєктів: {ex.Message}");
                }
            }
        }
    }
}

//    public class ProjectService : IProject
//    {
//        private List<Project> projects = new List<Project>();

//        public void CreateProject(string name, string description)
//        {
//            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be empty.");
//            var project = new Project(name, description);
//            projects.Add(project);
//        }

//        public void UpdateProject(Project project, string newName, string newDescription)
//        {
//            if (project == null) throw new ArgumentNullException(nameof(project));
//            if (!projects.Contains(project)) throw new ArgumentException("Project not found.");
//            if (!project.IsActive) throw new InvalidOperationException("Cannot update inactive project.");

//            if (!string.IsNullOrEmpty(newName)) project.Name = newName;
//            if (!string.IsNullOrEmpty(newDescription)) project.Description = newDescription;
//        }

//        public void DeleteProject(Project project)
//        {
//            if (project == null) throw new ArgumentNullException(nameof(project));
//            if (!projects.Contains(project)) throw new ArgumentException("Project not found.");
//            if (project.HasActiveTasks())
//                throw new InvalidOperationException("Cannot delete project with active tasks.");

//            projects.Remove(project);
//        }

//        public List<Project> GetProjects() => projects;
//    }
//}
