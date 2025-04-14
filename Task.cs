using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class Task
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime Deadline { get; private set; }
        public TaskStatus Status { get; private set; }
        public Developer AssignedDeveloper { get; private set; }
        public List<Comment> Comments { get; } = new List<Comment>();


        public Task(string title, string description, DateTime deadline)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException("Title cannot be empty.");
            if (deadline < DateTime.Now) throw new ArgumentException("Deadline must be in the future.");
            Title = title;
            Description = description ?? "";
            Deadline = deadline;
            Status = TaskStatus.Open;
        }

        public void Update(string title, string description)
        {
            if (Status == TaskStatus.Closed) throw new InvalidOperationException("Cannot update closed task.");
            if (!string.IsNullOrEmpty(title)) Title = title;
            if (description != null) Description = description;
        }

        public void SetStatus(TaskStatus status)
        {
            if (Status == TaskStatus.Closed) throw new InvalidOperationException("Cannot update closed task.");
            Status = status;
        }

        public void SetDeadline(DateTime deadline)
        {
            if (Status == TaskStatus.Closed) throw new InvalidOperationException("Cannot update closed task.");
            if (deadline < DateTime.Now) throw new ArgumentException("Deadline must be in the future.");
            Deadline = deadline;
        }

        public void AssignDeveloper(Developer developer)
        {
            if (Status == TaskStatus.Closed) throw new InvalidOperationException("Cannot assign developer to closed task.");
            AssignedDeveloper = developer;
        }

        public void AddComment(string text, User author)
        {
            if (Status == TaskStatus.Closed) throw new InvalidOperationException("Cannot comment on closed task.");
            Comments.Add(new Comment(text, author));
        }
    }
    public enum TaskStatus
    {
        Open,
        InProgress,
        Closed
    }
}