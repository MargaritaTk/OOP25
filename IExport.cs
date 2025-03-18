using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    using System.Text.Json;

    public interface IExport
    {
        string ExportProjects(List<Project> projects);
        IEnumerable<Project> ImportProjects(string jsonData);
    }
}
