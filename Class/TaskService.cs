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

    //public class TaskService : ITask
    //{
    //    public Task CreateTask(string title, string description, DateTime deadline)
    //    {
    //        if (string.IsNullOrEmpty(title)) throw new ArgumentException("Title cannot be empty.");
    //        if (deadline < DateTime.Now) throw new ArgumentException("Deadline must be in the future.");

    //        return new Task(title, description, deadline);
    //    }

    //    public void UpdateTask(Task task, string newTitle, string newDescription, DateTime newDeadline)
    //    {
    //        if (task == null) throw new ArgumentNullException(nameof(task));
    //        if (task.Status == TaskStatus.Closed)
    //            throw new InvalidOperationException("The task cannot be edited!");

    //        if (!string.IsNullOrEmpty(newTitle)) task.Title = newTitle;
    //        if (!string.IsNullOrEmpty(newDescription)) task.Description = newDescription;
    //        if (newDeadline > DateTime.Now) task.Deadline = newDeadline;
    //    }


    //    public void AssignDeveloper(Task task, Developer developer)
    //    {
    //        if (task == null) throw new ArgumentNullException(nameof(task));
    //        if (task.Status == TaskStatus.Closed)
    //            throw new InvalidOperationException("You cannot assign a developer to a closed task.");

    //        task.AssignedDeveloper = developer;
    //    }

    //    public void UpdateStatus(Task task, TaskStatus newStatus)
    //    {
    //        if (task == null) throw new ArgumentNullException(nameof(task));
    //        if (task.Status == TaskStatus.Closed)
    //            throw new InvalidOperationException("The status of a closed task cannot be updated.");

    //        task.UpdateStatus(newStatus);
    //    }
    //}
}
