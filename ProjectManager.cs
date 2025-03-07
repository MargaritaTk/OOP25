using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class ProjectManager : User, IProject, ITask, IComment, IExport
    {
        public ProjectManager(string login, string password) : base(login, password, Role.ProjectManager) { }

        public override void UpdateProfile(string login, string password) => throw new NotImplementedException();
        public void CreateProject(string name, string description) => throw new NotImplementedException();
        public void UpdateProject(Project project, string newName, string newDescription) => throw new NotImplementedException();
        public void DeleteProject(Project project) => throw new NotImplementedException();
        public Task CreateTask(string title, string description, DateTime deadline) => throw new NotImplementedException();
        public void UpdateTask(Task task, string newTitle, string newDescription, DateTime newDeadline) => throw new NotImplementedException();
        public void AssignDeveloper(Task task, Developer developer) => throw new NotImplementedException();
        public void UpdateStatus(Task task, TaskStatus newStatus) => throw new NotImplementedException();
        public void AddComment(Task task, string text) => throw new NotImplementedException();
        public string ExportProjects(List<Project> projects) => throw new NotImplementedException();
        public IEnumerable<Project> ImportProjects(string jsonData) => throw new NotImplementedException();
    }
}
