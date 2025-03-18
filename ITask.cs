using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public interface ITask
    {
        Task CreateTask(string title, string description, DateTime deadline);
        void UpdateTask(Task task, string newTitle, string newDescription, DateTime newDeadline);
        void AssignDeveloper(Task task, Developer developer);
        void UpdateStatus(Task task, TaskStatus newStatus);
    }
}
