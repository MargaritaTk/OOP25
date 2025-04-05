using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServ
{
    public interface IComment
    {
        void AddComment(Task task, string text);
    }
}
