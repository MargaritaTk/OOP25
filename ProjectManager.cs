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
        private ProjectService _projectService;
        private TaskService _taskService;
        private CommentService _commentService;
        private ExportService _exportService;
        private readonly UserRegistration _userRegistry;

        public ProjectManager(string login, string password, UserRegistration userRegistry, ProjectService projectService, TaskService taskService, CommentService commentService, ExportService exportService)
            : base(login, password, Role.ProjectManager)
        {
            _userRegistry = userRegistry ?? throw new ArgumentNullException(nameof(userRegistry));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));
        }

        public ProjectManager(string login, string password, UserRegistration userRegistry)
            : base(login, password, Role.ProjectManager)
        {
            _userRegistry = userRegistry ?? throw new ArgumentNullException(nameof(userRegistry));
        }

        public void InitializeServices(ProjectService projectService, TaskService taskService, CommentService commentService, ExportService exportService)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));
        }

        public Project CreateProject(string name, string description)
        {
            _projectService.CreateProject(name, description);
            return _projectService.GetProjects().Find(p => p.Name == name);
        }

        public void UpdateProject(Project project, string newName, string newDescription)
        {
            _projectService.UpdateProject(project, newName, newDescription);
        }

        public void DeleteProject(Project project)
        {
            _projectService.DeleteProject(project);
        }

        public Task CreateTask(string title, string description, DateTime deadline)
        {
            return _taskService.CreateTask(title, description, deadline);
        }

        public void UpdateTask(Task task, string newTitle, string newDescription)
        {
            _taskService.UpdateTask(task, newTitle, newDescription);
        }

        public void AssignDeveloper(Task task, Developer developer)
        {
            _taskService.AssignDeveloper(task, developer);
        }

        public void UpdateStatus(Task task, TaskStatus newStatus)
        {
            _taskService.UpdateStatus(task, newStatus);
        }

        public void UpdateDeadline(Task task, DateTime newDeadline)
        {
            _taskService.UpdateDeadline(task, newDeadline);
        }

        public void AddComment(Task task, string text)
        {
            _commentService.AddComment(task, text);
        }

        public void AddUser(User user)
        {
            _userRegistry.AddUser(user);
        }

        public void ExportProjects(List<Project> projects, string filePath)
        {
            _exportService.ExportProjectsToFile(projects, filePath);
        }

        public List<Project> ImportProjects(string filePath)
        {
            return _exportService.ImportProjectsFromFile(filePath);
        }
    }
}

    