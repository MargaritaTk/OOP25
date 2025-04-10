using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServ
{
    public class TaskService : ITask
    {
        public Task CreateTask(string title, string description, DateTime deadline)
        {
            return new Task(title, description, deadline);
        }

        public void UpdateTask(Task task, string newTitle, string newDescription)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            task.Update(newTitle, newDescription);
        }

        public void AssignDeveloper(Task task, Developer developer)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            task.AssignDeveloper(developer);
        }

        public void UpdateStatus(Task task, TaskStatus newStatus)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            task.SetStatus(newStatus);
        }

        public void UpdateDeadline(Task task, DateTime newDeadline)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            task.SetDeadline(newDeadline);
        }
    }
}
