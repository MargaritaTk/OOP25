using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public enum TaskStatus { Open, InProgress, Closed }

    public class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public TaskStatus Status { get; set; }
        public Developer AssignedDeveloper { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public virtual void UpdateStatus(TaskStatus newStatus) => throw new NotImplementedException();
        public event Action<TaskStatus> TaskStatusChanged;
    }
}
