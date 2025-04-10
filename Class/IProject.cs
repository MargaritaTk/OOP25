using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServ
{
    public interface IProject
    {
        void CreateProject(string name, string description);
        void UpdateProject(Project project, string newName, string newDescription);
        void DeleteProject(Project project);
        void AddTaskToProject(Project project, string title, string description, DateTime deadline, TaskStatus status, User assignedDeveloper);
        List<Project> GetProjects();
        List<Project> GetProjects(User user = null);
        void SaveProjects(IExport exportService);

    }
}
