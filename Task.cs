using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public КП_ООП.TaskStatus Status { get; set; }
        public Developer AssignedDeveloper { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public Task()
        {
            Title = "Default Task";
            Description = "";
            Deadline = DateTime.Now.AddDays(1);
            Status = КП_ООП.TaskStatus.Open;
        }

        public Task(string title, string description, DateTime deadline)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException("Title cannot be empty.");
            if (deadline < DateTime.Now) throw new ArgumentException("Deadline must be in the future.");
            Title = title;
            Description = description;
            Deadline = deadline;
            Status = КП_ООП.TaskStatus.Open;
        }

        public virtual void UpdateStatus(КП_ООП.TaskStatus newStatus)
        {
            if (Status == КП_ООП.TaskStatus.Closed) throw new InvalidOperationException("Cannot update closed task.");
            Status = newStatus;
            TaskStatusChanged?.Invoke(newStatus);
        }

        public event Action<КП_ООП.TaskStatus> TaskStatusChanged;
    }
    public enum TaskStatus
    {
        Open,
        InProgress,
        Closed
    }
}
