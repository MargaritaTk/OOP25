using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class Developer : User, ITask, IComment
    {
        private readonly CommentService _commentService;

        public Developer(string login, string password)
            : base(login, password, Role.Developer)
        {
            _commentService = new CommentService(this);
        }

        public Task CreateTask(string title, string description, DateTime deadline)
        {
            throw new InvalidOperationException("Developer cannot create tasks.");
        }

        public void UpdateTask(Task task, string newTitle, string newDescription)
        {
            throw new InvalidOperationException("Developer cannot update task details.");
        }

        public void AssignDeveloper(Task task, Developer developer)
        {
            throw new InvalidOperationException("Developer cannot assign tasks.");
        }

        public void UpdateStatus(Task task, TaskStatus newStatus)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.AssignedDeveloper != this) throw new InvalidOperationException("Can only update status of assigned tasks.");
            task.SetStatus(newStatus);
        }

        public void UpdateDeadline(Task task, DateTime newDeadline)
        {
            throw new InvalidOperationException("Developer cannot update deadlines.");
        }

        public void AddComment(Task task, string text)
        {
            _commentService.AddComment(task, text);
        }
    }


    //    public Developer(string login, string password, UserRegistration userRegistry)
    //    : base(login, password, Role.Developer)
    //    {
    //    }

    //    public override void UpdateProfile(string login, string password)
    //    {
    //        base.UpdateProfile(login, password);
    //    }

    //    public Task CreateTask(string title, string description, DateTime deadline)
    //    {
    //        if (string.IsNullOrEmpty(title)) throw new ArgumentException("Title cannot be empty.");
    //        if (deadline < DateTime.Now) throw new ArgumentException("Deadline must be in the future.");

    //        return new Task(title, description, deadline);
    //    }

    //    public void UpdateTask(КП_ООП.Task task, string newTitle, string newDescription, DateTime newDeadline)
    //    {
    //        if (task == null) throw new ArgumentNullException(nameof(task));
    //        if (task.Status == КП_ООП.TaskStatus.Closed)
    //            throw new InvalidOperationException("The task cannot be edited!");

    //        if (!string.IsNullOrEmpty(newTitle)) task.Title = newTitle;
    //        if (!string.IsNullOrEmpty(newDescription)) task.Description = newDescription;
    //    }

    //    public void AssignDeveloper(Task task, Developer developer)
    //    {
    //        if (task == null) throw new ArgumentNullException(nameof(task));
    //        if (task.Status == КП_ООП.TaskStatus.Closed)
    //            throw new InvalidOperationException("You cannot assign a developer to a closed task.");

    //        task.AssignedDeveloper = developer;
    //    }

    //    public void UpdateStatus(Task task, TaskStatus newStatus)
    //    {
    //        if (task == null) throw new ArgumentNullException(nameof(task));
    //        if (task.Status == TaskStatus.Closed)
    //            throw new InvalidOperationException("Cannot update status of closed task.");

    //        task.UpdateStatus(newStatus);
    //    }

    //    public void AddComment(КП_ООП.Task task, string text)
    //    {
    //        if (task == null) throw new ArgumentNullException(nameof(task));
    //        if (string.IsNullOrEmpty(text)) throw new ArgumentException("Comment text cannot be empty.");
    //        if (task.Status == TaskStatus.Closed)
    //            throw new InvalidOperationException("You cannot add a comment to a closed task.");

    //        task.Comments.Add(new Comment(text, this));
    //    }
    //}
}
