using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{

    public interface IExport
    {
            string ExportProjects(List<Project> projects);
            void ExportProjectsToFile(List<Project> projects, string filePath);
            void SaveToFile(List<Project> projects);

        }
    }
