﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectServ
{
    public class ExportService : IExport
    {
        public string ExportProjects(List<Project> projects)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            return JsonSerializer.Serialize(projects ?? throw new ArgumentNullException(nameof(projects)), options);
        }

        public List<Project> ImportProjects(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) throw new ArgumentException("JSON data cannot be empty.");
            return JsonSerializer.Deserialize<List<Project>>(jsonData) ?? new List<Project>();
        }
    }
}
