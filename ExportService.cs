using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class ExportService : IExport
    {
        private readonly ProjectService _projectService;

        public ExportService(ProjectService projectService)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        }

        public ExportService()
        {
        }

        public string ExportProjects(List<Project> projects)
        {
            if (projects == null) throw new ArgumentNullException(nameof(projects));
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            return JsonSerializer.Serialize(projects, options);
        }

        public List<Project> ImportProjects(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) throw new ArgumentException("JSON data cannot be empty.");
            return JsonSerializer.Deserialize<List<Project>>(jsonData) ?? new List<Project>();
        }

        public void ExportProjectsToFile(List<Project> projects, string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("File path cannot be empty.");
            string json = ExportProjects(projects);
            File.WriteAllText(filePath, json);
        }

        public List<Project> ImportProjectsFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("File path cannot be empty.");
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File {filePath} not found.");

            string json = File.ReadAllText(filePath);
            var projects = ImportProjects(json);

            if (_projectService != null)
            {
                _projectService.GetProjects().Clear();
                _projectService.GetProjects().AddRange(projects);
                _projectService.SaveToFile();
            }

            return projects;
        }
    }
}
   