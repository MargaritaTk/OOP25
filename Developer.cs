using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class Developer : User, ITask, IComment
    {
        public Developer(string login, string password) : base(login, password, Role.Developer) { }

        public override void UpdateProfile(string login, string password) => throw new NotImplementedException();
        public Task CreateTask(string title, string description, DateTime deadline) => throw new NotImplementedException();
        public void UpdateTask(Task task, string newTitle, string newDescription, DateTime newDeadline) => throw new NotImplementedException();
        public void AssignDeveloper(Task task, Developer developer) => throw new NotImplementedException();
        public void UpdateStatus(Task task, TaskStatus newStatus) => throw new NotImplementedException();
        public void AddComment(Task task, string text) => throw new NotImplementedException();
    }
}
