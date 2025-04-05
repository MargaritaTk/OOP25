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
}
