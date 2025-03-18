using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class ProjectManager : User, IProject, ITask, IComment, IExport
    {
        private List<Project> projects = new List<Project>();

        public ProjectManager(string login, string password, UserRegistration userRegistry)
        : base(login, password, Role.ProjectManager)
        {
        }

        public override void UpdateProfile(string login, string password)
        {
            base.UpdateProfile(login, password); 
        }

        public void CreateProject(string name, string description)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be empty.");

            var project = new Project(name, description);
            projects.Add(project);
        }
        public void UpdateProject(Project project, string newName, string newDescription)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (!projects.Contains(project)) throw new ArgumentException("Project not found.");
            if (!project.IsActive) throw new InvalidOperationException("Cannot update inactive project.");

            if (!string.IsNullOrEmpty(newName)) project.Name = newName;
            if (!string.IsNullOrEmpty(newDescription)) project.Description = newDescription;
        }

        public void DeleteProject(Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (!projects.Contains(project)) throw new ArgumentException("Project not found.");
            if (project.HasActiveTasks())
                throw new InvalidOperationException("Cannot delete project with active tasks.");

            projects.Remove(project);
        }

        public КП_ООП.Task CreateTask(string title, string description, DateTime deadline)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException("Title cannot be empty.");
            if (deadline < DateTime.Now) throw new ArgumentException("Deadline must be in the future.");

            var task = new КП_ООП.Task(title, description, deadline);
            return task;
        }

        public void UpdateTask(КП_ООП.Task task, string newTitle, string newDescription, DateTime newDeadline)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.Status == КП_ООП.TaskStatus.Closed)
                throw new InvalidOperationException("The task cannot be edited!");

            if (!string.IsNullOrEmpty(newTitle)) task.Title = newTitle;
            if (!string.IsNullOrEmpty(newDescription)) task.Description = newDescription;
            if (newDeadline > DateTime.Now) task.Deadline = newDeadline;
        }

        public void AssignDeveloper(КП_ООП.Task task, Developer developer)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.Status == КП_ООП.TaskStatus.Closed)
                throw new InvalidOperationException("You cannot assign a developer to a closed task.");

            task.AssignedDeveloper = developer;
        }

        public void UpdateStatus(КП_ООП.Task task, КП_ООП.TaskStatus newStatus)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.Status == КП_ООП.TaskStatus.Closed)
                throw new InvalidOperationException("The status of a closed task cannot be updated.");

            task.UpdateStatus(newStatus);
        }

        public void AddComment(КП_ООП.Task task, string text)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Comment text cannot be empty.");
            if (task.Status == КП_ООП.TaskStatus.Closed)
                throw new InvalidOperationException("You cannot add a comment to a closed task.");

            task.Comments.Add(new Comment(text, this));
        }

        public string ExportProjects(List<Project> projects)
        {
            if (projects == null) throw new ArgumentNullException(nameof(projects));
            return JsonSerializer.Serialize(projects);
        }

        public IEnumerable<Project> ImportProjects(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) throw new ArgumentException("JSON data cannot be empty.");
            return JsonSerializer.Deserialize<List<Project>>(jsonData) ?? new List<Project>();
        }
    }
}
   