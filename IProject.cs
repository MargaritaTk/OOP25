using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public interface IProject
    {
        void CreateProject(string name, string description);
        void UpdateProject(Project project, string newName, string newDescription);
        void DeleteProject(Project project);
    }
}
