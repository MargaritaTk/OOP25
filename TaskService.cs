using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class TaskService : ITask
    {
        public КП_ООП.Task CreateTask(string title, string description, DateTime deadline)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException("Title cannot be empty.");
            if (deadline < DateTime.Now) throw new ArgumentException("Deadline must be in the future.");

            return new КП_ООП.Task(title, description, deadline);
        }

        public void UpdateTask(КП_ООП.Task task, string newTitle, string newDescription, DateTime newDeadline)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.Status == КП_ООП.TaskStatus.Closed)
                throw new InvalidOperationException("The task cannot be edited!");

            if (!string.IsNullOrEmpty(newTitle)) task.Title = newTitle;
            if (!string.IsNullOrEmpty(newDescription)) task.Description = newDescription;
            if (newDeadline > DateTime.Now) task.Deadline = newDeadline;
        }

        public void AssignDeveloper(КП_ООП.Task task, Developer developer)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.Status == КП_ООП.TaskStatus.Closed)
                throw new InvalidOperationException("You cannot assign a developer to a closed task.");

            task.AssignedDeveloper = developer;
        }

        public void UpdateStatus(КП_ООП.Task task, КП_ООП.TaskStatus newStatus)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.Status == КП_ООП.TaskStatus.Closed)
                throw new InvalidOperationException("The status of a closed task cannot be updated.");

            task.UpdateStatus(newStatus);
        }
    }
}
