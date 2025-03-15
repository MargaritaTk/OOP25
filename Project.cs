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
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        private List<Task> Tasks { get; set; } = new List<Task>();

        public Project(string name, string description)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be empty.");
            Name = name;
            Description = description;
        }

        public void Activate()
        {
            if (!Tasks.Any() || Tasks.All(t => t.Status == КП_ООП.TaskStatus.Closed))
                IsActive = false;
            else
                IsActive = true;
        }

        public void Deactivate()
        {
            if (Tasks.Any(t => t.Status != КП_ООП.TaskStatus.Closed))
                throw new InvalidOperationException("Cannot deactivate with active tasks.");
            IsActive = false;
        }

        public static Project CreateDefault(string name) => new Project(name, "Default description");

        public IEnumerator<Task> GetEnumerator()
        {
            return Tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddTask(Task task)
        {
          Tasks.Add(task);
        }

        // Публичный метод для проверки активных задач
        public bool HasActiveTasks()
        {
            return Tasks.Any(t => t.Status != КП_ООП.TaskStatus.Closed);
        }
    }
}