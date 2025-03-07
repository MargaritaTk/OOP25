using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class Comment
    {
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public User Author { get; set; }

        public static Comment Create(string text, User author) => throw new NotImplementedException();
    }
}
