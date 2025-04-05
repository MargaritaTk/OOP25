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
        public string ExportProjects(List<Project> projects)
        {
            return JsonSerializer.Serialize(projects ?? throw new ArgumentNullException(nameof(projects)));
        }

        public List<Project> ImportProjects(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) throw new ArgumentException("JSON data cannot be empty.");
            return JsonSerializer.Deserialize<List<Project>>(jsonData) ?? new List<Project>();
        }
    }
}
