using System;
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

        public void Activate() => throw new NotImplementedException();
        public void Deactivate() => throw new NotImplementedException();
        public static Project CreateDefault(string name) => throw new NotImplementedException();

        public IEnumerator<Task> GetEnumerator() => Tasks.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
