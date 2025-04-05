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
        List<Project> ImportProjects(string jsonData);
    }
}
