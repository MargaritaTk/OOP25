using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class ProjectManager : User
    {
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private readonly ExportService _exportService;

        public ProjectManager(string login, string password, UserRegistration userRegistry)
            : base(login, password, Role.ProjectManager)
        {
            _projectService = new ProjectService();
            _taskService = new TaskService();
            _commentService = new CommentService(this);
            _exportService = new ExportService();
        }

        public override void UpdateProfile(string login, string password)
        {
            base.UpdateProfile(login, password);
        }

        public void CreateProject(string name, string description) => _projectService.CreateProject(name, description);
        public void UpdateProject(Project project, string newName, string newDescription) =>
            _projectService.UpdateProject(project, newName, newDescription);
        public void DeleteProject(Project project) => _projectService.DeleteProject(project);

        public КП_ООП.Task CreateTask(string title, string description, DateTime deadline) =>
            _taskService.CreateTask(title, description, deadline);
        public void UpdateTask(КП_ООП.Task task, string newTitle, string newDescription, DateTime newDeadline) =>
            _taskService.UpdateTask(task, newTitle, newDescription, newDeadline);
        public void AssignDeveloper(КП_ООП.Task task, Developer developer) => _taskService.AssignDeveloper(task, developer);
        public void UpdateStatus(КП_ООП.Task task, КП_ООП.TaskStatus newStatus) => _taskService.UpdateStatus(task, newStatus);

        public void AddComment(КП_ООП.Task task, string text) => _commentService.AddComment(task, text);

        public string ExportProjects(List<Project> projects) => _exportService.ExportProjects(projects);
        public IEnumerable<Project> ImportProjects(string jsonData) => _exportService.ImportProjects(jsonData);
    }
}
   
