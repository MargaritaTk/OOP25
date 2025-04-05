using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServ
{
    public class CommentService : IComment
    {
        private readonly User _author;

        public CommentService(User author)
        {
            _author = author ?? throw new ArgumentNullException(nameof(author));
        }

        public void AddComment(Task task, string text)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Comment text cannot be empty.");
            task.AddComment(text, _author);
        }
    }
        //public class CommentService : IComment
    //{
    //    private readonly User _author;

    //    public CommentService(User author)
    //    {
    //        _author = author ?? throw new ArgumentNullException(nameof(author));
    //    }

    //    public void AddComment(Task task, string text)
    //    {
    //        if (task == null) throw new ArgumentNullException(nameof(task));
    //        if (string.IsNullOrEmpty(text)) throw new ArgumentException("Comment text cannot be empty.");
    //        if (task.Status == TaskStatus.Closed)
    //            throw new InvalidOperationException("You cannot add a comment to a closed task.");

    //        task.Comments.Add(new Comment(text, _author));
    //    }
    //}
}
