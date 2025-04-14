using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectServ
{
    public class ExportService : IExport
    {
        private const string FilePath = "projects.json";

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

        public void ExportProjectsToFile(List<Project> projects, string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("File path cannot be empty.");
            string json = ExportProjects(projects);
            File.WriteAllText(filePath, json);
        }

        public void SaveToFile(List<Project> projects)
        {
            try
            {
                string json = ExportProjects(projects);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save projects to file: {ex.Message}", ex);
            }
        }
    }
}