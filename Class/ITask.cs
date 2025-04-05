using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServ
{
    public interface ITask
    {
        Task CreateTask(string title, string description, DateTime deadline);
        void UpdateTask(Task task, string newTitle, string newDescription);
        void AssignDeveloper(Task task, Developer developer);
        void UpdateStatus(Task task, TaskStatus newStatus);
        void UpdateDeadline(Task task, DateTime newDeadline);
    }
}