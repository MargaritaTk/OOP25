using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectServ
{
    public class ProjectManager : User
    {
        private ProjectService _projectService;
        private TaskService _taskService;
        private CommentService _commentService;
        private ExportService _exportService;
        private readonly UserRegistration _userRegistry;

        // Основний конструктор із усіма аргументами
        public ProjectManager(string login, string password, UserRegistration userRegistry, ProjectService projectService, TaskService taskService, CommentService commentService, ExportService exportService)
            : base(login, password, Role.ProjectManager)
        {
            _userRegistry = userRegistry ?? throw new ArgumentNullException(nameof(userRegistry));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));
        }

        // Перевантаження конструктора для десеріалізації та створення без сервісів
        public ProjectManager(string login, string password, UserRegistration userRegistry)
            : base(login, password, Role.ProjectManager)
        {
            _userRegistry = userRegistry ?? throw new ArgumentNullException(nameof(userRegistry));
            // Сервіси будуть ініціалізовані пізніше
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
            if (_projectService == null) throw new InvalidOperationException("ProjectService is not initialized.");
            _projectService.CreateProject(name, description);
            return _projectService.GetProjects().Find(p => p.Name == name);
        }

        public void UpdateProject(Project project, string newName, string newDescription)
        {
            if (_projectService == null) throw new InvalidOperationException("ProjectService is not initialized.");
            _projectService.UpdateProject(project, newName, newDescription);
        }

        public void DeleteProject(Project project)
        {
            if (_projectService == null) throw new InvalidOperationException("ProjectService is not initialized.");
            _projectService.DeleteProject(project);
        }

        public Task CreateTask(string title, string description, DateTime deadline)
        {
            if (_taskService == null) throw new InvalidOperationException("TaskService is not initialized.");
            return _taskService.CreateTask(title, description, deadline);
        }

        public void UpdateTask(Task task, string newTitle, string newDescription)
        {
            if (_taskService == null) throw new InvalidOperationException("TaskService is not initialized.");
            _taskService.UpdateTask(task, newTitle, newDescription);
        }

        public void AssignDeveloper(Task task, Developer developer)
        {
            if (_taskService == null) throw new InvalidOperationException("TaskService is not initialized.");
            _taskService.AssignDeveloper(task, developer);
        }

        public void UpdateStatus(Task task, TaskStatus newStatus)
        {
            if (_taskService == null) throw new InvalidOperationException("TaskService is not initialized.");
            _taskService.UpdateStatus(task, newStatus);
        }

        public void UpdateDeadline(Task task, DateTime newDeadline)
        {
            if (_taskService == null) throw new InvalidOperationException("TaskService is not initialized.");
            _taskService.UpdateDeadline(task, newDeadline);
        }

        public void AddComment(Task task, string text)
        {
            if (_commentService == null) throw new InvalidOperationException("CommentService is not initialized.");
            _commentService.AddComment(task, text);
        }

        public void AddUser(User user)
        {
            if (_userRegistry == null) throw new InvalidOperationException("UserRegistration is not initialized.");
            _userRegistry.AddUser(user);
        }

        public string ExportProjects()
        {
            if (_projectService == null || _exportService == null) throw new InvalidOperationException("ProjectService or ExportService is not initialized.");
            return _exportService.ExportProjects(_projectService.GetProjects());
        }

        public List<Project> ImportProjects(string jsonData)
        {
            if (_projectService == null || _exportService == null) throw new InvalidOperationException("ProjectService or ExportService is not initialized.");
            var projects = _exportService.ImportProjects(jsonData);
            _projectService.GetProjects().Clear();
            _projectService.GetProjects().AddRange(projects);
            return projects;
        }
    }
}


//    public class ProjectManager : User
//    {
//        private readonly ProjectService _projectService;
//        private readonly TaskService _taskService;
//        private readonly CommentService _commentService;
//        private readonly ExportService _exportService;
//        private readonly UserRegistration _userRegistry; // Добавляем поле

//        public UserRegistration UserRegistry => _userRegistry; // Свойство для десериализации

//        public ProjectManager(string login, string password, UserRegistration userRegistry)
//            : base(login, password, Role.ProjectManager)
//        {
//            _userRegistry = userRegistry;
//            _projectService = new ProjectService();
//            _taskService = new TaskService();
//            _commentService = new CommentService(this);
//            _exportService = new ExportService();
//        }

//        public override void UpdateProfile(string login, string password)
//        {
//            base.UpdateProfile(login, password);
//        }

//        public void CreateProject(string name, string description) => _projectService.CreateProject(name, description);
//        public void UpdateProject(Project project, string newName, string newDescription) =>
//            _projectService.UpdateProject(project, newName, newDescription);
//        public void DeleteProject(Project project) => _projectService.DeleteProject(project);

//        public Task CreateTask(string title, string description, DateTime deadline) =>
//            _taskService.CreateTask(title, description, deadline);
//        public void UpdateTask(Task task, string newTitle, string newDescription, DateTime newDeadline) =>
//            _taskService.UpdateTask(task, newTitle, newDescription, newDeadline);
//        public void AssignDeveloper(Task task, Developer developer) => _taskService.AssignDeveloper(task, developer);
//        public void UpdateStatus(Task task, TaskStatus newStatus) => _taskService.UpdateStatus(task, newStatus);

//        public void AddComment(Task task, string text) => _commentService.AddComment(task, text);

//        public string ExportProjects(List<Project> projects) => _exportService.ExportProjects(projects);
//        public IEnumerable<Project> ImportProjects(string jsonData) => _exportService.ImportProjects(jsonData);
//    }
//}

