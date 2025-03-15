using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КП_ООП
{
    public class Guest : User
    {
        public Guest(string login, string password) : base(login, password, Role.Guest) { }

        public override void UpdateProfile(string login, string password)
        {
            base.UpdateProfile(login, password);
        }
    }
}
