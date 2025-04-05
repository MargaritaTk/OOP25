using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class Project : IEnumerable<Task>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public List<Task> Tasks { get; } = new List<Task>();

        public Project(string name, string description)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be empty.");
            Name = name;
            Description = description ?? "";
            IsActive = true;
        }

        public void Update(string name, string description)
        {
            if (!IsActive) throw new InvalidOperationException("Cannot update inactive project.");
            if (!string.IsNullOrEmpty(name)) Name = name;
            if (description != null) Description = description;
        }

        public void Deactivate()
        {
            if (Tasks.Any(t => t.Status != TaskStatus.Closed))
                throw new InvalidOperationException("Cannot deactivate with active tasks.");
            IsActive = false;
        }

        public bool HasActiveTasks() => Tasks.Any(t => t.Status != TaskStatus.Closed);

        public IEnumerator<Task> GetEnumerator() => Tasks.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}