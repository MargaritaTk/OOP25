using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class ProjectService : IProject
    {
        private List<Project> projects = new List<Project>();

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

        public List<Project> GetProjects() => projects;
    }
}
